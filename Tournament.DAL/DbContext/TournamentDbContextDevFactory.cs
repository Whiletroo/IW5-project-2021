using System;
using Microsoft.EntityFrameworkCore;

namespace Tournament.DAL
{
    public class TournamentDbContextDevFactory : IDbContextFactory<TournamentDbContext>
    {
        public TournamentDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TournamentDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestDb");

            var dbContext = new TournamentDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
