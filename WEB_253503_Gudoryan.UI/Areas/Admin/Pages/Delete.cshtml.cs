﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.UI.Areas.Admin.Pages
{

    [Authorize(Policy = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IGameService _context;

        public DeleteModel(IGameService context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.GetGameByIdAsync(id.Value);

            if (game == null)
            {
                return NotFound();
            }

            if (game.Data == null)
            {
                return Unauthorized();
            }

            Game = game.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.GetGameByIdAsync(id.Value);
            if (game != null)
            {
                Game = game.Data;
                await _context.DeleteGameAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
