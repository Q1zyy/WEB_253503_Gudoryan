using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.Application.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public ApiCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories");
            var response = await _httpClient.GetAsync(new Uri(url.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
                } catch (JsonException ex)
                {
                    return ResponseData<List<Category>>.Error($"Warning {ex.Message}");
                }

            }

            return ResponseData<List<Category>>.Error($"No data from server, Error: {response.StatusCode.ToString()}");

        }
    }
}
