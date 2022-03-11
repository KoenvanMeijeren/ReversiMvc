// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using NUnit.Framework;
using ReversiMvc.Models.Entities;

namespace Tests.Model.Entities;

[TestFixture]
public class PlayerEntityTests
{
    [Test]
    public void CreatePlayerEntity_Empty()
    {
        // Arrange
        var player = new PlayerEntity();

        // Act

        // Assert
        Assert.IsNotEmpty(player.Guid);
        Assert.AreEqual(24, player.Guid.Length);
        Assert.AreEqual("", player.Name);
        Assert.AreEqual(0, player.Draws);
        Assert.AreEqual(0, player.Victories);
        Assert.AreEqual(0, player.Losses);
    }

    [Test]
    public void CreatePlayerEntity_NotEmpty()
    {
        // Arrange
        var player = new PlayerEntity
        {
            Guid = "test", Name = "Teddy", Draws = 1, Losses = 2, Victories = 10
        };

        // Act

        // Assert
        Assert.AreEqual("test", player.Guid);
        Assert.AreEqual("Teddy", player.Name);
        Assert.AreEqual(1, player.Draws);
        Assert.AreEqual(10, player.Victories);
        Assert.AreEqual(2, player.Losses);
    }
}
