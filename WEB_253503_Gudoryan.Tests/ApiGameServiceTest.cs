using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WEB_253503_Gudoryan.API.Controllers;
using WEB_253503_Gudoryan.API.Services.GameService;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.UI.Controllers;
using IGameService = WEB_253503_Gudoryan.API.Services.GameService.IGameService;
using ICategoryService = WEB_253503_Gudoryan.API.Services.CategoryService.ICategoryService;
using WEB_253503_Gudoryan.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.API.Data;

namespace WEB_253503_Gudoryan.Tests
{
	public class ApiGameServiceTest
	{
		private readonly AppDbContext _context;
		private readonly GameService _gameService;
		private List<Game> _games;

		public ApiGameServiceTest()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseSqlite("Filename=:memory:")
				.Options;

			_context = new AppDbContext(options);
			_context.Database.OpenConnection();
			_context.Database.EnsureCreated();

			_gameService = new GameService(_context);

			SeedData();
		}

		private void SeedData()
		{
			var categories = new List<Category>
			{
				new Category { Id = 1, Name = "Action", NormalizedName = "action" },
				new Category { Id = 2, Name = "Adventure",  NormalizedName = "adventure" }
			};

			_games = new List<Game>
			{
				new Game { Id = 1, Name = "Game 1", Category = categories[0], Description = "", Price = 0 },
				new Game { Id = 2, Name = "Game 2", Category = categories[0], Description = "", Price = 0 },
				new Game { Id = 3, Name = "Game 3", Category = categories[0], Description = "", Price = 0 },
				new Game { Id = 4, Name = "Game 4", Category = categories[1], Description = "", Price = 0 },
				new Game { Id = 5, Name = "Game 5", Category = categories[1], Description = "", Price = 0 },
			};

			_context.Categories.AddRange(categories);
			_context.Games.AddRange(_games);
			_context.SaveChanges();
		}

		[Fact]
		public async Task GetGameListAsynReturnsFirstPageOfThreeItems()
		{
			// Act
			var result = await _gameService.GetGameListAsync(null, 1);

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(3, result.Data?.Items.Count);
			Assert.Equal(2, result.Data.TotalPages);
			Assert.Equal(1, result.Data.CurrentPage);
		}	
		
		
		[Fact]
		public async Task GetGameListAsynReturnsSecondPageOfTwoItems()
		{
			// Act
			var result = await _gameService.GetGameListAsync(null, 2, 2);

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(2, result.Data?.Items.Count);
			Assert.Equal(3, result.Data.TotalPages);
			Assert.Equal(2, result.Data.CurrentPage);
		}		
		
		
		[Fact]
		public async Task GetGameListAsyncFilterByCategoryReturnsFilteredItems()
		{
			// Arrange
			var games = _games.Where(g => g?.Category?.NormalizedName == "adventure").ToList();
			
			// Act
			var result = await _gameService.GetGameListAsync("adventure");

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(2, result.Data?.Items.Count);
			Assert.Equal(1, result.Data.TotalPages);
			Assert.Equal(1, result.Data.CurrentPage);
			Assert.Equal(games, result.Data.Items);
		}

		[Fact]
		public async Task GetGameListAsyncPageNumberExceedsTotalPagesReturnsError()
		{
			// Act
			var result = await _gameService.GetGameListAsync(null, 3);

			// Assert
			Assert.False(result.Successful);
			Assert.Equal("No such page", result.ErrorMessage);
		}	
		
		
		[Fact]
		public async Task GetGameListAsyncPageSizeBiggerThanMaxPageSizeReturnsMaxPageSize()
		{
			// Act
			var result = await _gameService.GetGameListAsync(null, 1, 52);

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(1, result.Data?.TotalPages);
		}



	}
}
