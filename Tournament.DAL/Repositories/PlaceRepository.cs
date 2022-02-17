using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tournament.DAL.Entities;

namespace Tournament.DAL.Repositories
{
    public class PlaceRepository : RepositoryBase, IRepository<PlaceEntity>
    {
        public PlaceRepository(IDbContextFactory<TournamentDbContext> dbContextFactory) : base(dbContextFactory) { }

        public PlaceEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<PlaceEntity> query = dbContext.Set<PlaceEntity>();
            query = query.Include(entity => entity.Matches).ThenInclude(entity => entity.Team1);
            query = query.Include(entity => entity.Matches).ThenInclude(entity => entity.Team2);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<PlaceEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Places.Include(entity => entity.Matches).ToArray();
        }

        public PlaceEntity Insert(PlaceEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Places.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public PlaceEntity Update(PlaceEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Places.Any(p => p.Id == entity.Id))
            {
                dbContext.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Places.Any(p => p.Id == id))
            {
                var entity = new PlaceEntity
                {
                    Id = id
                };

                var matches = dbContext.Matches.Where(m => m.PlaceId == id).ToList();
                foreach (var match in matches)
                {
                    dbContext.Remove(match);
                }
                
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
