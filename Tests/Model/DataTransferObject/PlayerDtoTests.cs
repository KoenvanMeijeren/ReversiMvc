// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models.DataTransferObject;

namespace Tests.Model.DataTransferObject;

[TestFixture]
public class PlayerDtoTests
{

    [Test]
    public void PlayerJson_NotEmpty()
    {
        // Arrange
        var player = new PlayerJsonDto() {Color = "Black", Name = "Teddy", Token = "abcdef"};
        // Act

        // Assert
        Assert.AreEqual("Black", player.Color);
        Assert.AreEqual("Teddy", player.Name);
        Assert.AreEqual("abcdef", player.Token);
    }
    
    [Test]
    public void PlayerJson_Empty()
    {
        // Arrange
        var player = new PlayerJsonDto();
        // Act

        // Assert
        Assert.IsNull(player.Color);
        Assert.IsNull(player.Name);
        Assert.IsNull(player.Token);
    }
    
    [Test]
    public void Player_NotEmpty()
    {
        // Arrange
        var player = new PlayerDto(new PlayerJsonDto() {Color = "Black", Name = "Teddy", Token = "abcdef"});
        var player1 = new PlayerDto(new PlayerJsonDto() {Color = "White", Name = "Teddy", Token = "abcdef"});
        // Act

        // Assert
        Assert.AreEqual(Color.Black, player.Color);
        Assert.AreEqual(Color.White, player1.Color);
        Assert.AreEqual("Teddy", player.Name);
        Assert.AreEqual("abcdef", player.Token);
    }
    
    [Test]
    public void Player_Empty()
    {
        // Arrange
        var player = new PlayerDto(null);
        // Act

        // Assert
        Assert.AreEqual(Color.None , player.Color);
        Assert.IsNull(player.Name);
        Assert.IsNull(player.Token);
    }
    
}
