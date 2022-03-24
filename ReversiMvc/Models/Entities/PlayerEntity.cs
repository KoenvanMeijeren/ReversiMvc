// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace ReversiMvc.Models.Entities;

public class PlayerEntity : IEntity
{
    [Key]
    [Required]
    public string Guid { get; set; }

    [Required]
    public string Name { get; set; }

    public int Victories { get; set; }

    public int Losses { get; set; }

    public int Draws { get; set; }

    public PlayerEntity()
    {

    }

    public PlayerEntity(string guid = "", string name = "")
    {
        this.Guid = guid;
        if (string.IsNullOrEmpty(guid))
        {
            this.Guid = PlayerEntity.GenerateToken();
        }

        this.Name = name;
    }

    /// <summary>
    /// Generates the token for the game.
    ///
    /// Avoids using the '/' and '+' symbols, because the requests are done via API calls with the token as ID.
    /// </summary>
    /// <returns>The generated token.</returns>
    private static string GenerateToken()
    {
        return Convert
            .ToBase64String(System.Guid.NewGuid().ToByteArray())
            .Replace("/", "s")
            .Replace("=", "i")
            .Replace("?", "q")
            .Replace("+", "p");
    }

}
