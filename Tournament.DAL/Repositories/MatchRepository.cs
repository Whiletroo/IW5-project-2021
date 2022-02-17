using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.DAL.Entities;

namespace Tournament.DAL.Repositories
{
    public class MatchRepository : RepositoryBase, IRepository<MatchEntity>
    {
        public MatchRepository(IDbContextFactory<TournamentDbContext> dbContextFactory) : base(dbContextFactory) { }

        public MatchEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<MatchEntity> query = dbContext.Set<MatchEntity>();
            query = query.Include(entity => entity.Team1)
                .Include(entity => entity.Team2)
                .Include(entity => entity.Place);
            var entity = query.FirstOrDefault(entity => entity.Id == id)!;
            return entity;
        }

        public IEnumerable<MatchEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Matches.Include(entity => entity.Team1).Include(entity => entity.Team2).Include(entity => entity.Place).ToArray();
        }

        public MatchEntity Insert(MatchEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var place = dbContext.Places.FirstOrDefault(p => p.Id == entity.PlaceId);
            entity.Place = place;
            entity.PlaceId = place.Id;
            dbContext.Matches.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public MatchEntity Update(MatchEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Matches.Any(p => p.Id == entity.Id))
            {
                dbContext.Matches.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var entity = new MatchEntity
            {
                Id = id
            };
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
