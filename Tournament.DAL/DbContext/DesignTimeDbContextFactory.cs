using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tournament.DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TournamentDbContext>
    {
        public TournamentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TournamentDbContext>();
            var configuration = new ConfigurationBuilder().AddUserSecrets<TournamentDbContextFactory>(true).Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new TournamentDbContext(optionsBuilder.Options);
        }
    }
}
