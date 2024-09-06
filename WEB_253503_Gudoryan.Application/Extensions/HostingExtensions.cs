using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WEB_253503_Gudoryan.Application.Services.CategoryService;
using WEB_253503_Gudoryan.Application.Services.GameService;

namespace WEB_253503_Gudoryan.Application.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
			builder.Services.AddScoped<IGameService, MemoryGameService>();
		}
	}
}
