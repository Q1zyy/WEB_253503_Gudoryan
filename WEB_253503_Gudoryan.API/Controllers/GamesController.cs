﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.API.Data;
using WEB_253503_Gudoryan.API.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Domain.Models;

namespace WEB_253503_Gudoryan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGameService _gameService;

        public GamesController(AppDbContext context, IGameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(string? category, int pageNo = 1, int pageSize = 3)
        {
            var result = await _gameService.GetGameListAsync(category, pageNo, pageSize);
            if (result.Successful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Game>>> GetGame(int id)
        {
            var result = await _gameService.GetGameByIdAsync(id);
            if (result.Successful)
            {
                return Ok(result);
            } 
            return NotFound();
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            try
            {
                await _gameService.UpdateGameAsync(id, game);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _gameService.GetGameByIdAsync(id)).Successful)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseData<Game>>> PostGame(Game game)
        {
            var result = await _gameService.CreateGameAsync(game);
            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await _gameService.DeleteGameAsync(id);

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
