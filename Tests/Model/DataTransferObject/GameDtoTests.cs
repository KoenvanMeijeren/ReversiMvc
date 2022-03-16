// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models.DataTransferObject;

namespace Tests.Model.DataTransferObject;

[TestFixture]
public class GameDtoTests
{

    [Test]
    public void GameJson_NotEmpty()
    {
        // Arrange
        var game = new GameJsonDto()
        {
            Id = 1,
            Description = "test",
            Board = "[[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,1,2,0,0,0],[0,0,0,2,1,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0]]",
            Token = "abcdeff",
            PlayerOne = new PlayerJsonDto() { Color = "White", Name = "Teddy", Token = "qwerty"},
            PlayerTwo = new PlayerJsonDto() { Color = "Black", Name = "Jessica", Token = "zxcvb"},
            CurrentPlayer = new PlayerJsonDto() { Color = "Black", Name = "Jessica", Token = "zxcvb"},
            Status = "playing"
        };
        // Act

        // Assert
        Assert.AreEqual(1, game.Id);
        Assert.AreEqual("test", game.Description);
        Assert.AreEqual("abcdeff", game.Token);
        Assert.AreEqual("qwerty", game.PlayerOne.Token);
        Assert.AreEqual("zxcvb", game.PlayerTwo.Token);
        Assert.AreEqual("zxcvb", game.CurrentPlayer.Token);
        Assert.AreEqual("playing", game.Status);
    }
    
    [Test]
    public void GameJson_Empty()
    {
        // Arrange
        var game = new GameJsonDto();
        // Act

        // Assert
        Assert.AreEqual(0, game.Id);
        Assert.IsNull(game.Description);
        Assert.IsNull(game.Token);
        Assert.IsNull(game.PlayerOne);
        Assert.IsNull(game.PlayerTwo);
        Assert.IsNull(game.CurrentPlayer);
        Assert.IsNull(game.Board);
        Assert.IsNull(game.Status);
    }

}
