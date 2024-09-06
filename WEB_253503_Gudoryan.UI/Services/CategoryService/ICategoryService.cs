using WEB_253503_Gudoryan.Domain.Models;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.UI.Services.CategoryService
{
	public interface ICategoryService
	{
		/// <summary>
		/// Получение списка всех категорий
		/// </summary>
		/// <returns></returns>
		public Task<ResponseData<List<Category>>> GetCategoryListAsync();
	}
}
