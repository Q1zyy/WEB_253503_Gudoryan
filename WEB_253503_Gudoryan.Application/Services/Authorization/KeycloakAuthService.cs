﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEB_253503_Gudoryan.Application.Services.Authentication;
using WEB_253503_Gudoryan.Application.Services.FileService;
using WEB_253503_Gudoryan.Domain.HelperClasses;

namespace WEB_253503_Gudoryan.Application.Services.Authorization
{

    public class UserCredentials
    {
        public string Type { get; set; } = "password";
        public bool Temporary { get; set; } = false;
        public string Value { get; set; }
    }

    public class CreateUserModel
    {
        public Dictionary<string, string> Attributes { get; set; } = new();
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; } = true;
        public bool EmailVerified { get; set; } = true;
        public List<UserCredentials> Credentials { get; set; } = new();
    }


    public class KeycloakAuthService : IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;
        KeycloakData _keycloakData;
        public KeycloakAuthService(HttpClient httpClient,
                                   IOptions<KeycloakData> options,
                                   IFileService fileService,
                                   ITokenAccessor tokenAccessor
        )
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
            _keycloakData = options.Value;
        }


        public async Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar)
        {

            try
            {
                await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            var avatarUrl = "/Images/default-profile-picture.png";

            if (avatar != null)
            {
                var result = await _fileService.SaveFileAsync(avatar);
                if (result != null)
                {
                    avatarUrl = result;
                }
            }
            
            var newUser = new CreateUserModel();
            newUser.Attributes.Add("avatar", avatarUrl);
            newUser.Email = email;
            newUser.Username = email;
            newUser.Credentials.Add(new UserCredentials { Value = password });
            
            var requestUri = $"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users";
            
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var userData = JsonSerializer.Serialize(newUser, serializerOptions);
            
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(requestUri, content);
            if (response.IsSuccessStatusCode)
            {
                return (true, String.Empty);
            }
            return (false, response.StatusCode.ToString());
        }
    }
}