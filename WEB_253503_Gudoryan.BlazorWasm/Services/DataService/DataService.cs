using System.Net.Http.Json;
using System.Text;
using WEB_253503_Gudoryan.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using WEB_253503_Gudoryan.BlazorWasm.Pages;
using WEB_253503_Gudoryan.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace WEB_253503_Gudoryan.BlazorWasm.Services.DataService
{
    public class DataService : IDataService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenProvider _tokenProvider;

        private int _pageSize;

        public List<Category> Categories {  get; set; } = new List<Category>();
        public List<Game> Games { get; set; } = new List<Game>();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Category SelectedCategory { get; set; }

        public event Action DataLoaded;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _tokenProvider = accessTokenProvider;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        public async Task GetGameListAsync(int pageNo = 1)
        {
            var route = new StringBuilder("games/");
            // добавить категорию в маршрут
            if (SelectedCategory is not null)
            {
                route.Append($"{SelectedCategory.NormalizedName}");
            };
            List<KeyValuePair<string, string>> queryData = new();
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                queryData.Add(KeyValuePair.Create("pageNo", pageNo.ToString()));
            };
            // добавить размер страницы
            if (_pageSize != 3)
            {
                queryData.Add(KeyValuePair.Create("pageSize", _pageSize.ToString()));
            }
            // добавить строку запроса в Url
            if (queryData.Count > 0)
            {
                route.Append(QueryString.Create(queryData));
            }
            try
            {
                var tokenRequest = await _tokenProvider.RequestAccessToken();
                if (tokenRequest.TryGetToken(out var token))
                {
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
					var response = await _httpClient.GetAsync(_httpClient.BaseAddress + route.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var readed = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
                        Games = readed.Data.Items;
                        TotalPages = readed.Data.TotalPages;
                        CurrentPage = readed.Data.CurrentPage;

                        DataLoaded?.Invoke();
                        return;
                    }
                }
            }
            catch
            {
                Games = new List<Game>();
                Success = false;
                ErrorMessage = "Failed to load data.";
                DataLoaded?.Invoke();
            }
            Games = new List<Game>();
            Success = false;
            ErrorMessage = "Failed to load data.";
            DataLoaded?.Invoke();
        }

        public async Task GetCategoryListAsync()
        {
            var route = "categories";
            try
            {
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress + route);
                var readed = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
                Categories = readed.Data;
            }
            catch
            {
                Success = false;
                ErrorMessage = "Failed to load data.";
            }
        }
    }
}
