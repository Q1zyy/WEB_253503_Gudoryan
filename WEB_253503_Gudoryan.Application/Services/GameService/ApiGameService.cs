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
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB_253503_Gudoryan.Application.Services.GameService
{
    public class ApiGameService : IGameService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiGameService(HttpClient httpClient, IConfiguration configuration) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGameAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
