using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.Application.Services.CategoryService
{
	public class MemoryCategoryService : ICategoryService
	{
		public Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var categories = new List<Category>
{
				new Category {Id=1, Name="РПГ", NormalizedName="rpg"},
				new Category {Id=2, Name="Стратегии", NormalizedName="strategies"},
				new Category {Id=3, Name="Шутер", NormalizedName="shooters"},
				new Category {Id=4, Name="Гонки", NormalizedName="races"},
				new Category {Id=5, Name="Симуляторы", NormalizedName="simulators"},
				new Category {Id=6, Name="МОБА", NormalizedName="mobas"}
			};
			var result = ResponseData<List<Category>>.Success(categories);
			return Task.FromResult(result);
		}
	}
}
