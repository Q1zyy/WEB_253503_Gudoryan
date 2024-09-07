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
                await context.Categories.AddRangeAsync(
                    new List<Category>
                    {
                        new Category {Name="РПГ", NormalizedName="rpg"},
                        new Category {Name="Стратегии", NormalizedName="strategies"},
                        new Category {Name="Шутер", NormalizedName="shooters"},
                        new Category {Name="Гонки", NormalizedName="races"},
                        new Category {Name="Симуляторы", NormalizedName="simulators"},
                        new Category {Name="МОБА", NormalizedName="mobas"}
                    }
                );
                await context.SaveChangesAsync();
            }

            if (!(await context.Games.AnyAsync()))
            {
                var a = $"{baseUrl}/Images/dota2.jpg";
                var b = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("mobas"));
                await context.Games.AddRangeAsync(
                    new List<Game>
                    {
                        new Game
                        {
                            Name = "Dota 2",
                            Description = "Cool game",
                            Price = 0,
                            ImagePath = $"{baseUrl}/Images/dota2.jpg",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("mobas"))
                        },
                        new Game
                        {
                            Name = "CS 2",
                            Description = "Very cool game",
                            Price = 15,
                            ImagePath = $"{baseUrl}/Images/cs2.jpg",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("shooters"))
                        },
                        new Game
                        {
                            Name = "Dragon Age Origins",
                            Description = "This is rpg game",
                            Price = 25,
                            ImagePath = $"{baseUrl}/Images/dragonages.jpg",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("rpg"))
                        },
                        new Game
                        {
                            Name = "Forza Horizon 5",
                            Description = "The best races",
                            Price = 59.9M,
                            ImagePath = $"{baseUrl}/Images/forza.jpg",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("races"))
                        },
                        new Game
                        {
                            Name = "League of Legends",
                            Description = "Dota 2 is better",
                            Price = 0,
                            ImagePath = $"{baseUrl}/Images/lol.jpg",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("mobas"))
                        },
                        new Game
                        {
                            Name = "Hearts of Iron 4",
                            Description = "Real time strategy",
                            Price = 14.88M,
                            ImagePath = $"{baseUrl}/Images/hoi4.jfif",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("strategies"))
                        },
                        new Game
                        {
                            Name = "Farming simulator",
                            Description = "Yeah farmers",
                            Price = 29.99M,
                            ImagePath = $"{baseUrl}/Images/fm.jfif",
                            Category = await context.Categories.FirstAsync(c => c.NormalizedName.Equals("simulators"))
                        },

                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
