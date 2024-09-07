using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.API.Data
{
    public class DbInitializer
    {

        public static async Task SeedData(WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            string baseUrl = app.Configuration.GetValue<string>("BaseUrl");

            if (!(await context.Categories.AnyAsync()))
            {
                context.Categories.AddRange(
                     new List<Category>{
                        new Category {Name="РПГ", NormalizedName="rpg"},
                        new Category {Name="Стратегии", NormalizedName="strategies"},
                        new Category {Name="Шутер", NormalizedName="shooters"},
                        new Category {Name="Гонки", NormalizedName="races"},
                        new Category {Name="Симуляторы", NormalizedName="simulators"},
                        new Category {Name="МОБА", NormalizedName="mobas"}
                    }
                );
            }

            if (!(await context.Games.AnyAsync()))
            {

                var _games = new List<Game>
                {
                    new Game
                    {
                        Name = "Dota 2",
                        Description = "Cool game",
                        Price = 0,
                        ImagePath = "Images/dota2.jpg",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("mobas"))
                    },
                    new Game
                    {
                        Id = 2,
                        Name = "CS 2",
                        Description = "Very cool game",
                        Price = 15,
                        ImagePath = "Images/cs2.jpg",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("shooters"))
                    },
                    new Game
                    {
                        Id = 3,
                        Name = "Dragon Age Origins",
                        Description = "This is rpg game",
                        Price = 25,
                        ImagePath = "Images/dragonages.jpg",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("rpg"))
                    },
                    new Game
                    {
                        Id = 4,
                        Name = "Forza Horizon 5",
                        Description = "The best races",
                        Price = 59.9M,
                        ImagePath = "Images/forza.jpg",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("races"))
                    },
                    new Game
                    {
                        Id = 5,
                        Name = "League of Legendsa",
                        Description = "Dota 2 is better",
                        Price = 0,
                        ImagePath = "Images/lol.jpg",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("mobas"))
                    },
                    new Game
                    {
                        Id = 6,
                        Name = "Hearts of Iron 4",
                        Description = "Real time strategy",
                        Price = 14.88M,
                        ImagePath = "Images/hoi4.jfif",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("strategies"))
                    },
                    new Game
                    {
                        Id = 7,
                        Name = "Farming simulator",
                        Description = "Yeah farmers",
                        Price = 29.99M,
                        ImagePath = "Images/fm.jfif",
                        Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("simulators"))
                    },

                };

            }
        }
    }
}
