// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReversiMvc.Data;
using ReversiMvc.Models.Entities;
using ReversiMvc.Repository;
using ReversiMvc.Repository.Contracts;

namespace Tests.Repository;

[TestFixture]
public class PlayersDatabaseRepositoryTests
{
    private readonly IPlayersRepository _repository;

    public PlayersDatabaseRepositoryTests()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the
        // connection is closed at the end of the test (see Dispose below).
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened
        // above.
        var contextOptions = new DbContextOptionsBuilder<ReversiDbContext>()
            .UseSqlite(connection)
            .Options;

        // Create the schema and seed some data
        var context = new ReversiDbContext(contextOptions);

        context.Database.EnsureCreated();

        context.Players.AddRange(
            new PlayerEntity {Guid = "abcdef", Name = "Teddy", Draws = 1},
            new PlayerEntity {Guid = "uiop", Name = "John", Victories = 2},
            new PlayerEntity {Guid = "test", Name = "Jessica", Losses = 2}
        );
        context.SaveChanges();

        this._repository = new PlayersRepository(context);
    }

    [Test]
    public void All()
    {
        var players = this._repository.All();

        Assert.AreEqual(4, players.Count());
        Assert.AreEqual("abcdef", players.First().Guid);
    }

    [Test]
    public void Add()
    {
        // Arrange

        // Act
        this._repository.Add(new PlayerEntity("qwerty"));
        var players = this._repository.All();

        // Assert
        Assert.AreEqual(4, players.Count());
        Assert.AreEqual("qwerty", players.Last().Guid);
    }

    [Test]
    public void FirstOrCreate()
    {
        // Arrange
        Assert.AreEqual(3, this._repository.All().Count());

        // Act
        var player = this._repository.FirstOrCreate(new PlayerEntity("hjikl"));
        player = this._repository.FirstOrCreate(new PlayerEntity("hjikl"));
        var players = this._repository.All();

        // Assert
        Assert.AreEqual(4, players.Count());
        Assert.AreEqual("hjikl", player.Guid);
    }

    [Test]
    public void Update()
    {
        // Arrange
        var player = this._repository.Get("test");

        // Act
        var result = this._repository.Update(player);
        
        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Delete()
    {
        // Arrange
        var player = this._repository.Get("qwerty");
        
        // Act
        var result = this._repository.Delete(player);
        
        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3, this._repository.All().Count());
        Assert.IsNull(this._repository.Get("qwerty"));
    }
    
    [Test]
    public void Exists_True()
    {
        // Arrange

        // Act

        // Assert
        Assert.IsTrue(this._repository.Exists("abcdef"));
        Assert.IsTrue(this._repository.Exists("uiop"));
        Assert.IsTrue(this._repository.Exists("test"));
    }

    [Test]
    public void Exists_False()
    {
        // Arrange

        // Act

        // Assert
        Assert.IsFalse(this._repository.Exists("fdafd"));
        Assert.IsFalse(this._repository.Exists("vczdas"));
        Assert.IsFalse(this._repository.Exists(null));
    }
    
    [Test]
    public void GetDbSet()
    {
        // Arrange

        // Act

        // Assert
        Assert.IsInstanceOf<DbSet<PlayerEntity>>(this._repository.GetDbSet());
    }
}
