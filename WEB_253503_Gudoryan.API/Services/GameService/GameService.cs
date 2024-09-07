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

        public Task<ResponseData<Game>> CreateProductAsync(Game product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Category).FirstAsync(g => g.Id == id);
            return ResponseData<Game>.Success(game);
        }

        public async Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }

            var data = _context.Games
                .Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();


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

        public Task UpdateProductAsync(int id, Game product)
        {
            throw new NotImplementedException();
        }
    }
}
