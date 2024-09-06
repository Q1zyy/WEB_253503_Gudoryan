using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;
using WEB_253503_Gudoryan.Application.Services.CategoryService;

namespace WEB_253503_Gudoryan.Application.Services.GameService
{
	public class MemoryGameService : IGameService
	{

		List<Game> _games;
		List<Category> _categories;
		private readonly IConfiguration _config;

		public MemoryGameService([FromServices] IConfiguration config,  ICategoryService categoryService)
		{ 
			_categories = categoryService.GetCategoryListAsync().Result.Data;
			_config = config;
			SetupDate();
		}

		private void SetupDate()
		{
			_games = new List<Game>
			{
				new Game
				{
					Id = 1,
					Name = "Dota 2",
					Description = "Cool game",
					Price = 0,
					ImagePath = "Images/dota2.jpg",
					Category = _categories.Find(c => c.NormalizedName.Equals("mobas"))
				},
				new Game
				{
                    Id = 2,
                    Name = "CS 2",
                    Description = "Very cool game",
                    Price = 15,
                    ImagePath = "Images/cs2.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("shooters"))
                },
				new Game
				{
                    Id = 3,
                    Name = "Dragon Age Origins",
                    Description = "This is rpg game",
                    Price = 25,
                    ImagePath = "Images/dragonages.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("rpg"))
                },
				new Game
				{
                    Id = 4,
                    Name = "Forza Horizon 5",
                    Description = "The best races",
                    Price = 59.9M,
                    ImagePath = "Images/forza.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("races"))
                },
				new Game
				{
                    Id = 5,
                    Name = "League of Legendsa",
                    Description = "Dota 2 is better",
                    Price = 0,
                    ImagePath = "Images/lol.jpg",
                    Category = _categories.Find(c => c.NormalizedName.Equals("mobas"))
                },
				new Game
				{
                    Id = 6,
                    Name = "Hearts of Iron 4",
                    Description = "Real time strategy",
                    Price = 14.88M,
                    ImagePath = "Images/hoi4.jfif",
                    Category = _categories.Find(c => c.NormalizedName.Equals("strategies"))
                },
				new Game
				{
                    Id = 7,
                    Name = "Farming simulator",
                    Description = "Yeah farmers",
                    Price = 29.99M,
                    ImagePath = "Images/fm.jfif",
                    Category = _categories.Find(c => c.NormalizedName.Equals("simulators"))
                },

			};
		}

		public Task<ResponseData<Game>> CreateGameAsync(Game game, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

		public Task DeleteGameAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<Game>> GetGameByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1)
		{
			int itemsPerPage = _config.GetValue<int>("ItemsPerPage");
            var data = _games
				.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
				.ToList();
            int allPages = (data.Count + itemsPerPage - 1) / itemsPerPage;
			int skip = (pageNo - 1) * itemsPerPage;
			int canTake = Math.Min((data.Count - skip), itemsPerPage);
            return Task.FromResult(ResponseData<ListModel<Game>>.Success(new ListModel<Game>
			{
				Items = data.Skip(skip).Take(canTake).ToList(),
				CurrentPage = pageNo,
				TotalPages = allPages,
			})) ;
		}

		public Task UpdateGameAsync(int id, Game game, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}
	}
}
