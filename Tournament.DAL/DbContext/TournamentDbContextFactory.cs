using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tournament.DAL
{
    public class TournamentDbContextFactory : IDbContextFactory<TournamentDbContext>
    {
        private readonly IConfiguration _config;

        public TournamentDbContextFactory(IConfiguration config) : base()
        {
            _config = config;
        }

        public TournamentDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TournamentDbContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            return new TournamentDbContext(optionsBuilder.Options);
        }
    }
}
