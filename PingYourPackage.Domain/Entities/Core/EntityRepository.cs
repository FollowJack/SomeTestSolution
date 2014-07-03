using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities.Core
{
    public class EntityRepository<T> : IEntityRepository<T> : where T : class, IEntity, new()
    {
        private readonly DbContext _entitiesContext;

        public EntityRepository(DbContext entitiesContext)
        {
            if (entitiesContext == null)
                throw new ArgumentNullException("entitiesContext");

            _entitiesContext = entitiesContext;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entitiesContext.Set<T>();
        } 
    }
}
