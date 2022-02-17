using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.DAL.Entities;

namespace Tournament.DAL.Repositories
{
    public class TeamRepository : RepositoryBase, IRepository<TeamEntity>

    {
        public TeamRepository(IDbContextFactory<TournamentDbContext> dbContextFactory) : base(dbContextFactory) { }

        public TeamEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<TeamEntity> query = dbContext.Set<TeamEntity>();
            query = query.Include(entity => entity.Persons);
            var entity = query.FirstOrDefault(entity => entity.Id == id)!;
            return entity;
        }

        public IEnumerable<TeamEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Teams.Include(entity => entity.Persons).ToArray();
        }

        public TeamEntity Insert(TeamEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Teams.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public TeamEntity Update(TeamEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Teams.Any(p => p.Id == entity.Id))
            {

                dbContext.Teams.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }


        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Teams.Any(p => p.Id == id))
            {
                var entity = new TeamEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
