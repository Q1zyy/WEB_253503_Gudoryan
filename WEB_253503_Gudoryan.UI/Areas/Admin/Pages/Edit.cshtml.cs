using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.Application.Services.CategoryService;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IGameService _context;
        private readonly ICategoryService _categoryService;
        public List<Category> Categories { get; set; }

        public EditModel(IGameService context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;


        [BindProperty]
        public IFormFile? ImagePath { get; set; }

        [BindProperty]
        public int Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Categories = (await _categoryService.GetCategoryListAsync()).Data;
            var game =  await _context.GetGameByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }
            if (game.Data == null)
            {
                return NotFound();
            }
            Game = game.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Categories = (await _categoryService.GetCategoryListAsync()).Data;
            Game.Category = Categories.FirstOrDefault(c => c.Id == Category);

            await _context.UpdateGameAsync(Game.Id, Game, ImagePath);

            return RedirectToPage("./Index");
        }

        private bool GameExists(int id)
        {
            return true;
            //return _context.Games.Any(e => e.Id == id);
        }
    }
}
