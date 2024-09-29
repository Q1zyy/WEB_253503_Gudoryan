using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Gudoryan.Application.Services.Authentication
{
    public interface ITokenAccessor
    {
        /// <summary>
        /// Получение access-token
        /// </summary>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync();
        /// <summary>
        /// Добавление заголовка Authorization : bearer
        /// </summary>
        /// <param name="httpClient">HttpClient, в который добавляется заголовок</param>
        /// <returns></returns>
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);

    }
}
