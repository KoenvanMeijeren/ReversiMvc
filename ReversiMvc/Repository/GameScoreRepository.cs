using Microsoft.AspNetCore.Identity;

namespace ReversiMvc.Repository;

/// <summary>
/// Provides a repository for the users.
/// </summary>
public class GameScoreRepository : RepositoryDatabaseBase<GameScoreEntity>
{
    public GameScoreRepository(ReversiDbContext context) : base(context, context.GameScores)
    {

    }
}
