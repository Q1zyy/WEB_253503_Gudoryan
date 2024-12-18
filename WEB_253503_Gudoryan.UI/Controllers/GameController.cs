﻿using Microsoft.AspNetCore.Mvc;
using WEB_253503_Gudoryan.Application.Extensions;
using WEB_253503_Gudoryan.Application.Services.CategoryService;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.UI.Controllers
{
	public class GameController : Controller
	{

		private readonly IGameService _gameService;
		private List<Category> _categories;

		public GameController(ICategoryService categoryService, IGameService gameService)
		{
			_gameService = gameService;
			_categories = categoryService.GetCategoryListAsync().Result.Data;
		}

		[HttpGet("games/{category?}")]
		public async Task<IActionResult> Index(string? category, int pageNo = 1)
		{
			Category? category1 = _categories.Find(c => c.NormalizedName.Equals(category));

			if (category != null)
			{
				if (category1 == null)
				{
					return NotFound();
				}
			}

			ViewData["currentCategory"] = (category == null) ? "Все" :category1.Name;
			ViewData["currentCategoryShortName"] = category;
			ViewBag.Categories = _categories;
			var productResponse = await _gameService.GetGameListAsync(category, pageNo);
			
			if (!productResponse.Successful)
			{
				return NotFound(productResponse.ErrorMessage);
			}            
			
			var result = new ListModel<Game>
			{
				Items = productResponse.Data.Items,
				CurrentPage = pageNo,
				TotalPages = productResponse.Data.TotalPages
            };

            if (Request.IsAjaxRequest())
			{ 
                return PartialView("_GameListPartial", result);
            }

            return View(result);
		}
	}
}
