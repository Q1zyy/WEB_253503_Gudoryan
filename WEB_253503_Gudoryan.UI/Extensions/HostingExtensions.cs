using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WEB_253503_Gudoryan.UI.Services.CategoryService;
using WEB_253503_Gudoryan.UI.Services.GameService;

namespace WEB_253503_Gudoryan.UI.Extensions
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
