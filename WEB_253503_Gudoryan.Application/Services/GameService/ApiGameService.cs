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

        public ApiGameService(HttpClient httpClient, IConfiguration configuration, IFileService fileService) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
        {
            // Первоначально использовать картинку по умолчанию
            game.ImagePath = "Images/noimage.jpg";
            // Сохранить файл изображения
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                // Добавить в объект Url изображения
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    game.ImagePath = imageUrl;
                }
            }
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Games");
            var response = await _httpClient.PostAsJsonAsync(uri, game);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Game>>();
                return data;
            }
        
            return ResponseData<Game>.Error($"Объект не добавлен. Error { response.StatusCode.ToString()}");
        }

        public Task DeleteGameAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var url = $"{_httpClient.BaseAddress}games/{id}";
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

        public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
