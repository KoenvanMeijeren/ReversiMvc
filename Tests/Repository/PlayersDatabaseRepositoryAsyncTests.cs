// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReversiMvc.Data;
using ReversiMvc.Models.Entities;
using ReversiMvc.Repository;
using ReversiMvc.Repository.Contracts;

namespace Tests.Repository;

[TestFixture]
public class PlayersDatabaseRepositoryAsyncTests
{
    private readonly IPlayersRepository _repository;

    public PlayersDatabaseRepositoryAsyncTests()
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
            new PlayerEntity { Guid = "abcdef", Name = "Teddy", Draws = 1 },
            new PlayerEntity { Guid = "uiop", Name = "John", Victories = 2 },
            new PlayerEntity { Guid = "test", Name = "Jessica", Losses = 2 }
        );
        context.SaveChanges();

        this._repository = new PlayersRepository(context);
    }

    [Test(ExpectedResult = 1)]
    public async Task<int> All()
    {
        var players = await this._repository.AllAsync();

        Assert.AreEqual(4, players.Count());
        Assert.AreEqual("abcdef", players.First().Guid);

        return 1;
    }

    [Test(ExpectedResult = 1)]
    public async Task<int> Add()
    {
        // Arrange

        // Act
        await this._repository.AddAsync(new PlayerEntity("qwerty"));
        var players = this._repository.All();

        // Assert
        Assert.AreEqual(4, players.Count());
        Assert.AreEqual("qwerty", players.Last().Guid);

        return 1;
    }

    [Test(ExpectedResult = 1)]
    public async Task<int> Update()
    {
        // Arrange
        var player = this._repository.Get("test");

        // Act
        var result = await this._repository.UpdateAsync(player);

        // Assert

        return result;
    }

    [Test(ExpectedResult = 1)]
    public async Task<int> Delete()
    {
        // Arrange
        var player = this._repository.Get("qwerty");

        // Act
        var result = await this._repository.DeleteAsync(player);

        // Assert
        Assert.AreEqual(3, this._repository.All().Count());
        Assert.IsNull(this._repository.Get("qwerty"));

        return result;
    }
}
