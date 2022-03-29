// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Newtonsoft.Json;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Models;

public class GameEditViewModel
{

    public const string UndefinedValue = "-";
    
    private readonly GameJsonDto? _gameJsonDto;

    public int? Id => this._gameJsonDto?.Id;
    public string? Description => this._gameJsonDto?.Description;
    public string? Token => this._gameJsonDto?.Token;
    public PlayerDto PlayerOne => new PlayerDto(this._gameJsonDto?.PlayerOne);
    public PlayerDto PlayerTwo => new PlayerDto(this._gameJsonDto?.PlayerTwo);
    public PlayerDto CurrentPlayer => new PlayerDto(this._gameJsonDto?.CurrentPlayer);
    public PlayerEntity LoggedInPlayer { get; }
    public PlayerDto DominantPlayerDto { get; }
    public PlayerDto LoserPlayerDto { get; }

    public string? Opponent { get; }
    public string? PredominantColor { get; }
    public string? PredominantPlayer { get; }
    
    public Color[,] Board
    {
        get
        {
            if (this._gameJsonDto?.Board == null)
            {
                return new Color[1, 1];
            }

            return JsonConvert.DeserializeObject<Color[,]>(this._gameJsonDto.Board)!;
        }
    }

    public Status Status => this._gameJsonDto?.Status?.ToLower() switch
    {
        "created" => Status.Created,
        "queued" => Status.Queued,
        "pending" => Status.Pending,
        "playing" => Status.Playing,
        "finished" => Status.Finished,
        "quit" => Status.Quit,
        _ => Status.Created
    };

    public GameEditViewModel()
    {

    }

    public GameEditViewModel(GameJsonDto? gameJsonDto = null, PlayerEntity? loggedInPlayer = null)
    {
        this._gameJsonDto = gameJsonDto;
        this.PredominantColor = gameJsonDto?.PredominantColor;
        this.PredominantPlayer = UndefinedValue;
        if (gameJsonDto == null || loggedInPlayer == null)
        {
            return;
        }

        this.LoggedInPlayer = loggedInPlayer;
        this.Opponent = this.PlayerOne.Name;
        if (!loggedInPlayer.Guid.Equals(this.PlayerTwo.Token))
        {
            this.Opponent = this.PlayerTwo.Name;
        }

        if (!this.Status.Equals(Status.Finished) && !this.Status.Equals(Status.Quit))
        {
            return;
        }
        
        if (Color.White.ToString().Equals(gameJsonDto.PredominantColor))
        {
            this.PredominantPlayer = "Tegenstander";
            if (this.LoggedInPlayer.Guid.Equals(this.PlayerOne.Token))
            {
                this.PredominantPlayer = "Ik";
            }

            this.DominantPlayerDto = this.PlayerOne;
            this.LoserPlayerDto = this.PlayerTwo;
        }
        else if (Color.Black.ToString().Equals(gameJsonDto.PredominantColor))
        {
            this.PredominantPlayer = "Tegenstander";
            if (this.LoggedInPlayer.Guid.Equals(this.PlayerTwo.Token))
            {
                this.PredominantPlayer = "Ik";
            }
            
            this.DominantPlayerDto = this.PlayerTwo;
            this.LoserPlayerDto = this.PlayerOne;
        }
    }

    public bool CanAddPlayerOne()
    {
        if (this.LoggedInPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.PlayerOne.Token == null && !this.LoggedInPlayer.Guid.Equals(this.PlayerTwo.Token);
    }

    public bool CanAddPlayerTwo()
    {
        if (this.LoggedInPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.PlayerTwo.Token == null && !this.LoggedInPlayer.Guid.Equals(this.PlayerOne.Token);
    }

    public bool CanStart()
    {
        if (this.PlayerOne.Token == null || this.PlayerTwo.Token == null)
        {
            return false;
        }

        return this.Status.Equals(Status.Pending)
               && this.LoggedInPlayer is { Guid: { } }
               && this.LoggedInPlayer.Guid.Equals(this.PlayerOne.Token);
    }

    public bool CanQuit()
    {
        if (this.LoggedInPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.Status.Equals(Status.Playing)
               && (this.LoggedInPlayer.Guid.Equals(this.PlayerOne.Token)
                   || this.LoggedInPlayer.Guid.Equals(this.PlayerTwo.Token));
    }

    public bool IsLoggedInPlayerOwner()
    {
        return this.LoggedInPlayer is { Guid: { } }
               && this.LoggedInPlayer.Guid.Equals(this.PlayerOne.Token);
    }

    public bool IsPending()
    {
        return this.Status.Equals(Status.Pending);
    }

    public bool IsPlaying()
    {
        return this.Status.Equals(Status.Playing);
    }

    public bool IsQuit()
    {
        return this.Status.Equals(Status.Quit);
    }

    public bool IsFinished()
    {
        return this.Status.Equals(Status.Finished);
    }


    public bool IsPlayingOrEnded()
    {
        return this.IsPlaying() || this.IsQuit() || this.IsFinished();
    }

}
