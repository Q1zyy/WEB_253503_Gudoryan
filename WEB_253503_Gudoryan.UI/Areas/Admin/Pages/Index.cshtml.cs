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
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.UI.Areas.Admin.Pages
{


    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IGameService _context;

        public IndexModel(IGameService context)
        {
            _context = context;
        }

        public ListModel<Game> Game { get;set; } = default!;

        public async Task OnGetAsync(int pageNo = 1)
        {
            Game = (await _context.GetGameListAsync(null, pageNo)).Data;
        }
    }
}
