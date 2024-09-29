﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WEB_253503_Gudoryan.Domain.HelperClasses;

namespace WEB_253503_Gudoryan.Application.Services.Authentication
{
    public class KeycloakTokenAccessor : ITokenAccessor
    {
        private readonly KeycloakData _keycloakData;
        private readonly HttpContext _httpContext;
        private readonly HttpClient _httpClient;

        public KeycloakTokenAccessor(
            IHttpContextAccessor httpContextAccessor,
            IOptions<KeycloakData> options,
            HttpClient httpClient
        )
        {
            _httpContext = httpContextAccessor.HttpContext;
            _keycloakData = options.Value;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                return await _httpContext.GetTokenAsync("access_token");
            }

            var requestUri = $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";

            HttpContent content = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret)
            ]);

            var response = await _httpClient.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonObject.Parse(jsonString)["access_token"].GetValue<string>();
        }

        public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            string token = await GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
        }
    }
}
