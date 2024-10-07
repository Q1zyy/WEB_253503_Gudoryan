using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Gudoryan.Application.Services.CategoryService;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.UI.Areas.Admin.Pages
{

    [Authorize(Policy = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IGameService _context;
        private readonly ICategoryService _categoryService;

        public List<Category> Categories { get; set; }

        public CreateModel(IGameService context, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            Categories = (await _categoryService.GetCategoryListAsync()).Data;
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImagePath { get; set; }

        [BindProperty]
        public int Category {  get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
             Categories = (await _categoryService.GetCategoryListAsync()).Data;
            Game.Category = Categories.FirstOrDefault(c => c.Id == Category);
            
            await _context.CreateGameAsync(Game, ImagePath);

            return RedirectToPage("./Index");
        }
    }
}
