using Microsoft.AspNetCore.Identity;

namespace ReversiMvc.Repository;

/// <summary>
/// Provides a repository for the users.
/// </summary>
public class UsersRepository : RepositoryDatabaseBase<IdentityUser>
{
    public UsersRepository(ApplicationDbContext context) : base(context, context.Users)
    {

    }
}
