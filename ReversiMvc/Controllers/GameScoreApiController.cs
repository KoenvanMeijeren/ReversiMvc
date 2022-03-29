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
    private readonly IRepository<GameScoreEntity> _gameScoreRepository;

    public GameScoreController(IPlayersRepository playersRepository, IGameRepository gameRepository, IRepository<GameScoreEntity> gameScoreRepository)
    {
        this._playersRepository = playersRepository;
        this._gameRepository = gameRepository;
        this._gameScoreRepository = gameScoreRepository;
    }

    [HttpGet("{token}")]
    public async Task<ActionResult> GameScore(string? token)
    {
        var entity = await this._gameRepository.Get(token);
        if (entity == null)
        {
            return this.NotFound();
        }

        var gameScoreEntity = this._gameScoreRepository.All()
            .FirstOrDefault(score => score.GameToken != null && score.GameToken.Equals(entity.Token));
        if (gameScoreEntity != null)
        {
            return this.Ok();
        }

        var gameViewModel = new GameEditViewModel(entity);

        var dominantColor = gameViewModel.PredominantColor;
        var playerOne = this._playersRepository.Get(gameViewModel.PlayerOne.Token);
        var playerTwo = this._playersRepository.Get(gameViewModel.PlayerTwo.Token);
        if (playerTwo == null || playerOne == null || dominantColor is null or GameEditViewModel.UndefinedValue)
        {
            return this.BadRequest();
        }

        this._playersRepository.UpdatePlayerScores(dominantColor, playerOne, playerTwo);
        this._gameScoreRepository.Add(new GameScoreEntity(entity.Token));

        return this.Ok();
    }
}
