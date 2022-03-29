// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Models.Entities;

public class GameScoreEntity
{
    [Key]
    public int Id { get; set; }

    public string? GameToken { get; set; }

    public GameScoreEntity() : this(string.Empty)
    {
        
    }

    public GameScoreEntity(string? gameToken)
    {
        this.GameToken = gameToken;
    }
}
