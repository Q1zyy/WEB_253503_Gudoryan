using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WEB_253503_Gudoryan.API.Controllers;
using WEB_253503_Gudoryan.API.Services.GameService;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.UI.Controllers;
using IGameService = WEB_253503_Gudoryan.Application.Services.GameService.IGameService;
using ICategoryService = WEB_253503_Gudoryan.Application.Services.CategoryService.ICategoryService;
using WEB_253503_Gudoryan.Domain.Models;
using Microsoft.AspNetCore.Http;


namespace WEB_253503_Gudoryan.Tests
{
	public class GameControllerTest
	{

		private readonly GameController _controller;
		private readonly IGameService _gameService;
		private readonly ICategoryService _categoryService;
		private readonly List<Category> _categories;


		public GameControllerTest()
		{
			_gameService = Substitute.For<IGameService>();
			_categoryService = Substitute.For<ICategoryService>();
			_categories = new List<Category>
			{		
				new Category { Name = "strategy", NormalizedName = "strategies" },
				new Category { Name = "rpg", NormalizedName = "rpgs" }
			};
			_categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>> { Data = _categories, Successful = true });
			_controller = new GameController(_categoryService, _gameService);
		}


		[Fact]
		public async Task IndexCategoryNotFoundReturnsNotFound()
		{
			// Arrange
			string category = "aboba123";

			// Act
			var result = await _controller.Index(category);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}	
		
		
		[Fact]
		public async Task IndexGetGameListAsyncFailsReturnsNotFoundWithErrorMessage()
		{
			// Arrange
			string category = "rpg";
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Successful = false,
				}
			); 

			// Act
			var result = await _controller.Index(category);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}	
		
		
		[Fact]
		public async Task IndexViewDataContainsCategories()
		{
			// Arrange
			string category = "rpgs";
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Data = new ListModel<Game> { },
					Successful = true,
				}
			);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};


			// Act
			var result = await _controller.Index(category);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.IsType<List<Category>>(viewResult.ViewData["Categories"]);
		}	
		
		
		[Fact]
		public async Task IndexViewDataContainsTheCategory()
		{
			// Arrange
			string category = "rpgs";
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Data = new ListModel<Game> { },
					Successful = true,
				}
			);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};


			// Act
			var result = await _controller.Index(category);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal(viewResult.ViewData["currentCategoryShortName"], category);
			Assert.Equal(viewResult.ViewData["currentCategory"], "rpg");
		}	
		
		
		[Fact]
		public async Task IndexViewDataContainsCategoryAll()
		{
			// Arrange
			string? category = null;
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Data = new ListModel<Game> { },
					Successful = true,
				}
			);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};


			// Act
			var result = await _controller.Index(category);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal(viewResult.ViewData["currentCategoryShortName"], category);
			Assert.Equal(viewResult.ViewData["currentCategory"], "Все");
		}	
		
		
		
		[Fact]
		public async Task IndexViewReturnsModel()
		{
			// Arrange
			string? category = null;
			var games = new List<Game> { };
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Data = new ListModel<Game> { Items = games },
					Successful = true,
				}
			);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};


			// Act
			var result = await _controller.Index(category);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<ListModel<Game>>(viewResult.Model);
			Assert.Equal(games, model.Items);
		}
			
		
		
		[Fact]
		public async Task IndexAjaxRequestReturnsPartialView()
		{
			// Arrange
			string? category = null;
			var games = new List<Game> { };
			_gameService.GetGameListAsync(category, Arg.Any<int>()).Returns(
				new ResponseData<ListModel<Game>>
				{
					Data = new ListModel<Game> { Items = games },
					Successful = true,
				}
			);
			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers["x-requested-with"] = "xmlhttprequest";
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};


			// Act
			var result = await _controller.Index(category);

			// Assert
			Assert.IsType<PartialViewResult>(result);
			
		}


	}
}