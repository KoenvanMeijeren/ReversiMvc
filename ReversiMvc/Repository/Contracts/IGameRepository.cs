using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Repository.Contracts;

/// <summary>
/// Provides an interface for games repository.
/// </summary>
public interface IGameRepository : IAsyncRepository<GameJsonDto>
{

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    new Task<GameJsonDto?> AddAsync(GameJsonDto entity);

    /// <summary>
    /// Adds a player entity to the game.
    /// </summary>
    /// <param name="token">The token of the game.</param>
    /// <param name="playerGuid">The guid of the player.</param>
    /// <param name="playerName">The name of the player.</param>
    Task<GameJsonDto?> AddPlayerOneAsync(string token, string playerGuid, string playerName);

    /// <summary>
    /// Adds a player entity to the game.
    /// </summary>
    /// <param name="token">The token of the game.</param>
    /// <param name="playerGuid">The guid of the player.</param>
    /// <param name="playerName">The name of the player.</param>
    Task<GameJsonDto?> AddPlayerTwoAsync(string token, string playerGuid, string playerName);

    /// <summary>
    /// Starts the game.
    /// </summary>
    /// <param name="token">The token of the game.</param>
    Task<GameJsonDto?> StartAsync(string token);

    /// <summary>
    /// Quits the game.
    /// </summary>
    /// <param name="token">The token of the game.</param>
    Task<GameJsonDto?> QuitAsync(string token);

    /// <summary>
    /// Determines if the game exists.
    /// </summary>
    /// <param name="token">The unique token of the game.</param>
    /// <returns>Whether the game exists or not.</returns>
    Task<bool> Exists(string? token);

    /// <summary>
    /// Gets the game by the token.
    /// </summary>
    /// <param name="token">The unique token of the game.</param>
    /// <returns>The game.</returns>
    Task<GameJsonDto?> Get(string? token);

    /// <summary>
    /// Gets the game by the token of the player.
    /// </summary>
    /// <param name="token">The unique token of the player.</param>
    /// <returns>The game.</returns>
    Task<GameJsonDto?> GetByPlayerToken(string? token);

}
