using System;
using Microsoft.EntityFrameworkCore;

namespace Tournament.DAL.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly IDbContextFactory<TournamentDbContext> _dbContextFactory;

        public RepositoryBase(IDbContextFactory<TournamentDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
    }
}
