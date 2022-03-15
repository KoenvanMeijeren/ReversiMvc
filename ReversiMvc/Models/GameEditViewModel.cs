// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Newtonsoft.Json;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Models;

public class GameEditViewModel
{

    private readonly GameJsonDto? _gameJsonDto;
    
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
                return new Color[1,1];
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

    public GameEditViewModel() : this(null)
    {
        
    }
    
    public GameEditViewModel(GameJsonDto? gameJsonDto = null)
    {
        this._gameJsonDto = gameJsonDto;
    }
    
    public bool CanAddPlayerOne()
    {
        return this.PlayerOne.Token == null;
    }
    
    public bool CanAddPlayerTwo()
    {
        return this.PlayerTwo.Token == null;
    }

    public bool CanStart()
    {
        return this.Status.Equals(Status.Pending);
    }

    public bool CanQuit()
    {
        return this.Status.Equals(Status.Playing);
    }

}
