using ReversiMvc.Models.DataTransferObject;

namespace ReversiMvc.Repository;

/// <summary>
/// Provides a repository for the game.
/// </summary>
public class PlayersRepository : RepositoryDatabaseBase<PlayerEntity>, IPlayersRepository
{

    private readonly DbSet<PlayerEntity> _players;

    public PlayersRepository(ReversiDbContext context) : base(context, context.Players)
    {
        this._players = context.Players;
    }

    /// <inheritdoc />
    public PlayerEntity FirstOrCreate(PlayerEntity playerEntity)
    {
        var dbPlayer = this.Get(playerEntity.Guid);
        if (dbPlayer != null)
        {
            return dbPlayer;
        }

        this.Add(playerEntity);

        return playerEntity;
    }

    /// <inheritdoc />
    public bool Exists(string? guid)
    {
        return this.Get(guid) != null;
    }

    /// <inheritdoc />
    public PlayerEntity? Get(string? guid)
    {
        return this._players.SingleOrDefault(player => player.Guid.Equals(guid));
    }

    /// <inheritdoc />
    public DbSet<PlayerEntity> GetDbSet()
    {
        return this._players;
    }

    /// <inheritdoc />
    public void UpdatePlayerScores(string dominantColor, PlayerEntity playerOne, PlayerEntity playerTwo)
    {
        if (Color.None.ToString().Equals(dominantColor))
        {
            playerOne.Draws++;
            playerTwo.Draws++;
        }
        else if (Color.White.ToString().Equals(dominantColor))
        {
            playerOne.Victories++;
            playerTwo.Losses++;
        }
        else if (Color.Black.ToString().Equals(dominantColor))
        {
            playerOne.Losses++;
            playerTwo.Victories++;
        }

        this.Context.Entry(playerOne).State = EntityState.Modified;
        this.Context.Entry(playerTwo).State = EntityState.Modified;

        this.Context.SaveChanges();
    }

}
