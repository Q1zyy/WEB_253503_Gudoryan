﻿using Microsoft.AspNetCore.Http;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.Application.Services.GameService
{
	public interface IGameService
	{
		/// <summary>
		/// Получение списка всех объектов
		/// </summary>
		/// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
		/// <param name="pageNo">номер страницы списка</param>
		/// <returns></returns>
		public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1);
		/// <summary>
		/// Поиск объекта по Id
		/// </summary>
		/// <param name="id">Идентификатор объекта</param>
		/// <returns>Найденный объект или null, если объект не найден</returns>
		public Task<ResponseData<Game>> GetGameByIdAsync(int id);
		/// <summary>
		/// Обновление объекта
		/// </summary>
		/// <param name="id">Id изменяемомго объекта</param>
		/// <param name="game">объект с новыми параметрами</param>
		/// <param name="formFile">Файл изображения</param>
		/// <returns></returns>
		public Task UpdateGameAsync(int id, Game game, IFormFile? formFile);
		/// <summary>
		/// Удаление объекта
		/// </summary>
		/// <param name="id">Id удаляемомго объекта</param>
		/// <returns></returns>
		public Task DeleteGameAsync(int id);
		/// <summary>
		/// Создание объекта
		/// </summary>
		/// <param name="game">Новый объект</param>
		/// <param name="formFile">Файл изображения</param>
		/// <returns>Созданный объект</returns>
		public Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile);
	}
}
