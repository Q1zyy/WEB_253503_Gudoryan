using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEB_253503_Gudoryan.Application.Services.Authentication;
using WEB_253503_Gudoryan.Application.Services.FileService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB_253503_Gudoryan.Application.Services.GameService
{
    public class ApiGameService : IGameService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiGameService(
            HttpClient httpClient,
            IConfiguration configuration,
            IFileService fileService,
            ITokenAccessor tokenAccessor
        ) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
        {
            // Первоначально использовать картинку по умолчанию
            game.ImagePath = "Images/noimage.jpg";
            // Сохранить файл изображения
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    game.ImagePath = imageUrl;
                }
            }
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Games");


            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.PostAsJsonAsync(uri, game);
            
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>();
                return data;
            }
        
            return ResponseData<Game>.Error($"Объект не добавлен. Error { response.StatusCode.ToString()}");
        }

        public async Task DeleteGameAsync(int id)
        {
            var curGame = await GetGameByIdAsync(id);
            await _fileService.DeleteFileAsync(curGame.Data.ImagePath);
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var url = $"{_httpClient.BaseAddress}games/{id}";
            var response = await _httpClient.DeleteAsync(new Uri(url));
        }

        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var url = $"{_httpClient.BaseAddress}games/{id}";
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Game>>();
                }
                catch (JsonException ex)
                {
                    return ResponseData<Game>.Error($"Error {ex.Message}");
                }
            }

            return ResponseData<Game>.Error($"Данные не получены от сервера. Error:{response.StatusCode.ToString()}");

        }

        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1)
        {

            var url = new StringBuilder($"{_httpClient.BaseAddress}games/");
            int itemsPerPage = _configuration.GetValue<int>("ItemsPerPage");
            
            if (categoryNormalizedName != null)
            {
                url.Append($"{categoryNormalizedName}/");
            }

            StringBuilder query = new StringBuilder();


            if (pageNo > 1)
            {
                query.Append($"pageNo={pageNo}");
            }
            
            if (!itemsPerPage.Equals("3"))
            {
                if (query.Length > 0)
                {
                    query.Append('&');
                }
                query.Append($"pageSize={itemsPerPage}");
            }

            url.Append($"?{query}");

            var response = await _httpClient.GetAsync(new Uri(url.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>();
                }
                catch (JsonException ex)
                {
                    return ResponseData<ListModel<Game>>.Error($"Error {ex.Message}");
                }
            }

            return ResponseData<ListModel<Game>>.Error($"Данные не получены от сервера. Error:{response.StatusCode.ToString()}");

        }

        public async Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            var curGame = await GetGameByIdAsync(id);
            if (!curGame.Successful)
            {
                return ;
            }


            await _fileService.DeleteFileAsync(curGame.Data.ImagePath);


            var baseUrl = _configuration.GetSection("BaseApiUrl").Value;
            game.ImagePath = $"{baseUrl}Images/noimage.jpg";
            
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    game.ImagePath = imageUrl;
                }
            }
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Games/{id}");

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.PutAsJsonAsync(uri, game);
        }
    }


}
