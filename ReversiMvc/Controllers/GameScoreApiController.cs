using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameScoreController : ControllerBase
{
    private readonly IPlayersRepository _playersRepository;
    private readonly IGameRepository _gameRepository;

    public GameScoreController(IPlayersRepository playersRepository, IGameRepository gameRepository)
    {
        this._playersRepository = playersRepository;
        this._gameRepository = gameRepository;
    }
    
    [HttpGet("{token}")]
    public async Task<ActionResult> GameScore(string? token)
    {
        var entity = await this._gameRepository.Get(token);
        if (entity == null)
        {
            return this.NotFound();
        }

        var gameViewModel = new GameEditViewModel(entity);
        
        var dominantColor = gameViewModel.PredominantColor;
        var playerOne = this._playersRepository.Get(gameViewModel.PlayerOne.Token);
        var playerTwo = this._playersRepository.Get(gameViewModel.PlayerTwo.Token);
        if (playerTwo == null || playerOne == null || dominantColor == null || dominantColor == GameEditViewModel.UndefinedValue)
        {
            return this.BadRequest();
        }
        
        this._playersRepository.UpdatePlayerScores(dominantColor, playerOne, playerTwo);

        return this.Ok();
    }
}
