using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ReversiMvc.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
