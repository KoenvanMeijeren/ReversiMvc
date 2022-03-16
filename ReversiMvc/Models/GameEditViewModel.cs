// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Newtonsoft.Json;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Models;

public class GameEditViewModel
{

    private readonly GameJsonDto? _gameJsonDto;
    private readonly PlayerEntity? _currentPlayer;

    public int? Id => this._gameJsonDto?.Id;
    public string? Description => this._gameJsonDto?.Description;
    public string? Token => this._gameJsonDto?.Token;
    public PlayerDto PlayerOne => new PlayerDto(this._gameJsonDto?.PlayerOne);
    public PlayerDto PlayerTwo => new PlayerDto(this._gameJsonDto?.PlayerTwo);
    public PlayerDto CurrentPlayer => new PlayerDto(this._gameJsonDto?.CurrentPlayer);

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

    public Status Status => this._gameJsonDto?.Status switch
    {
        "Created" => Status.Created,
        "Queued" => Status.Queued,
        "Pending" => Status.Pending,
        "Playing" => Status.Playing,
        "Finished" => Status.Finished,
        "Quit" => Status.Quit,
        _ => Status.Created
    };

    public GameEditViewModel()
    {

    }

    public GameEditViewModel(GameJsonDto? gameJsonDto = null, PlayerEntity currentPlayer = null)
    {
        this._gameJsonDto = gameJsonDto;
        this._currentPlayer = currentPlayer;
    }

    public bool CanAddPlayerOne()
    {
        if (this._currentPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.PlayerOne.Token == null && !this._currentPlayer.Guid.Equals(this.PlayerTwo.Token);
    }

    public bool CanAddPlayerTwo()
    {
        if (this._currentPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.PlayerTwo.Token == null && !this._currentPlayer.Guid.Equals(this.PlayerOne.Token);
    }

    public bool CanStart()
    {
        return this.Status.Equals(Status.Pending)
               && this._currentPlayer is { Guid: { } }
               && this._currentPlayer.Guid.Equals(this.PlayerOne.Token);
    }

    public bool CanQuit()
    {
        if (this._currentPlayer is not { Guid: { } })
        {
            return false;
        }

        return this.Status.Equals(Status.Playing)
               && (this._currentPlayer.Guid.Equals(this.PlayerOne.Token)
                   || this._currentPlayer.Guid.Equals(this.PlayerTwo.Token));
    }

}
