namespace ReversiMvc.Repository;

/// <summary>
/// Provides a repository for the game.
/// </summary>
public class PlayersDatabaseRepository : RepositoryDatabaseBase<PlayerEntity>, IPlayersRepository
{

    private readonly DbSet<PlayerEntity> _players;
    
    public PlayersDatabaseRepository(ReversiDbContext context) : base(context, context.Players)
    {
        this._players = context.Players;
    }

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
}
