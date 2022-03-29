// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace ReversiMvc.Data;

public class ReversiDbContext : DbContext
{
    public DbSet<PlayerEntity> Players { get; set; }

    public DbSet<GameScoreEntity> GameScores { get; set; }

    public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options)
    {
    }
}
