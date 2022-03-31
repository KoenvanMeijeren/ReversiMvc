using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Controllers;

[Route("api/Game")]
[ApiController]
public class GameApiController : ControllerBase
{
    private readonly IGameRepository _gameRepository;

    public GameApiController(IGameRepository gameRepository)
    {
        this._gameRepository = gameRepository;
    }

    [HttpGet("{token}")]
    public async Task<ActionResult> GetByToken(string? token)
    {
        return this.Ok(await this._gameRepository.Get(token));
    }

    [HttpPut("do-move")]
    public async Task<ActionResult> DoMoveGame([FromBody] GameDoMoveDto? gameDoMove)
    {
        return this.Ok(await this._gameRepository.DoMoveAsync(gameDoMove));
    }

    [HttpPut("{token}/quit")]
    public async Task<ActionResult> QuitGame(string? token)
    {
        return this.Ok(await this._gameRepository.QuitAsync(token));
    }
}
