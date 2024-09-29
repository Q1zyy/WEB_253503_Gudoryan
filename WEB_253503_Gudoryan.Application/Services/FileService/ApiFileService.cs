using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_Gudoryan.Application.Services.Authentication;

namespace WEB_253503_Gudoryan.Application.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
            _httpContext = new HttpContextAccessor().HttpContext;
        }

        public async Task DeleteFileAsync(string fileUri)
        {
            if (string.IsNullOrWhiteSpace(fileUri))
            {
                return;
            }

            string fileName = fileUri.Split('/').Last();
            Uri uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Files");

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var content = new MultipartFormDataContent
            {
                { new StringContent(fileName), "fileName" }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = uri,
                Content = content
            };


            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);
            
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Files");

            request.Content = content;

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.PostAsync(uri.ToString(), request.Content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }
    }
}
