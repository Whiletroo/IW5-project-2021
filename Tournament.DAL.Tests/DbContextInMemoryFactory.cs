using System;
using Microsoft.EntityFrameworkCore;

namespace Tournament.DAL.Tests
{
    class DbContextInMemoryFactory : IDbContextFactory<TournamentDbContext>
    {
        private readonly string _databaseName;

        public DbContextInMemoryFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public TournamentDbContext CreateDbContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<TournamentDbContext>();
            contextOptionsBuilder.UseInMemoryDatabase(_databaseName);
            return new TournamentDbContext(contextOptionsBuilder.Options);
        }
    }
}
