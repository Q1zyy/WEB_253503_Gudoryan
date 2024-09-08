using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.API.Data;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.API.Services.GameService
{
    public class GameService : IGameService
    {

        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<Game>> CreateGameAsync(Game game)
        {
            var category = await _context.Categories.FindAsync(game.Category.Id);
            if (category != null)
            {
                game.Category = category;
            }
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return ResponseData<Game>.Success(game); 
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            } 
        }

        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Category).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return ResponseData<Game>.Error("No such game");
            }
            return ResponseData<Game>.Success(game);
        }

        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }

            var data = await _context.Games.Include(g => g.Category)
                .Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToListAsync();


            int allPages = (data.Count + pageSize - 1) / pageSize;
            int skip = (pageNo - 1) * pageSize;
            int canTake = Math.Min((data.Count - skip), pageSize);

            if (pageNo > allPages)
            {
                return ResponseData<ListModel<Game>>.Error("No such page");
            }


            return ResponseData<ListModel<Game>>.Success(new ListModel<Game>
            {
                Items = data.Skip(skip).Take(canTake).ToList(),
                CurrentPage = pageNo,
                TotalPages = allPages,
            });
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateGameAsync(int id, Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
