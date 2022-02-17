using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.DAL.Entities;

namespace Tournament.DAL.Repositories
{
    public class PersonRepository : RepositoryBase, IRepository<PersonEntity>
    {
        public PersonRepository(IDbContextFactory<TournamentDbContext> dbContextFactory) : base(dbContextFactory) { }

        public PersonEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<PersonEntity> query = dbContext.Set<PersonEntity>();
            query = query.Include(entity => entity.Team);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<PersonEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Persons.ToArray();
        }

        public PersonEntity Insert(PersonEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Persons.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public PersonEntity Update(PersonEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Persons.Any(p => p.Id == entity.Id))
            {
                dbContext.Persons.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Persons.Any(p => p.Id == id))
            {
                var entity = new PersonEntity
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
