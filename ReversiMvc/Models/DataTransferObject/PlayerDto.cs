// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Models.DataTransferObject;

public class PlayerDto
{

    private readonly PlayerJsonDto? _playerJsonDto;

    public string? Token => this._playerJsonDto?.Token;

    public Color Color => this._playerJsonDto?.Color switch
    {
        "White" => Color.White,
        "Black" => Color.Black,
        _ => Color.None
    };

    public string? Name => this._playerJsonDto?.Name;

    public PlayerDto(PlayerJsonDto? playerJsonDto)
    {
        this._playerJsonDto = playerJsonDto;
    }
}
