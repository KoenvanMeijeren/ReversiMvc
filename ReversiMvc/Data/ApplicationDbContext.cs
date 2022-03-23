using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ReversiMvc.Data;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // any guid
        const string AdminId = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
        // any guid, but nothing is against to use the same one
        const string RoleId = "37186721-a6e7-418f-8a56-1c3b6596264f";
        
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = RoleId,
                Name = ApplicationRoleTypes.Admin , 
                NormalizedName = ApplicationRoleTypes.Admin.ToUpper()
            },
            new IdentityRole
            {
                Name = ApplicationRoleTypes.Mediator , 
                NormalizedName = ApplicationRoleTypes.Mediator.ToUpper()
            }
        );
        
        var hasher = new PasswordHasher<IdentityUser>();
        builder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = AdminId,
            UserName = "admin@nimda.com",
            NormalizedUserName = "ADMIN@NIMDA.COM",
            Email = "admin@nimda.com",
            NormalizedEmail = "ADMIN@NIMDA.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "nimda"),
            SecurityStamp = string.Empty
        });
        
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = RoleId,
            UserId = AdminId
        });
    }
}
