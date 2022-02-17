using System;
using System.Collections.Generic;

namespace Tournament.DAL.Repositories
{
    public interface IRepository<Entity>
    {
        Entity Get(Guid id);
        IEnumerable<Entity> GetAll();
        Entity Insert(Entity entity);
        Entity Update(Entity entity);
        void Delete(Guid id);
    }
}
