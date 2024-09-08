using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEB_253503_Gudoryan.Application.Services.CategoryService;
using WEB_253503_Gudoryan.Application.Services.GameService;

namespace WEB_253503_Gudoryan.Application.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(this WebApplicationBuilder builder)
		{
            var apiUri = builder.Configuration.GetSection("UriData").Get<UriData>();

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(apiUri.ApiUri));

			builder.Services.AddHttpClient<IGameService, ApiGameService>(opt => opt.BaseAddress = new Uri(apiUri.ApiUri));

		}
	}
}
