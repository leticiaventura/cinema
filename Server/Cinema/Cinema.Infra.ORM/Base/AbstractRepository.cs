using System.Data.Entity;
using System.Linq;
using Cinema.Domain.Common;

namespace Cinema.Infra.ORM.Base
{
    /// <summary>
    /// Classe de respositório abstrata para ser utilizada pelos repositórios. Contém operações básicas de CRUD.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractRepository<T> : IRepository<T> where T : Entity
    {
        protected BaseContext _context;

        public AbstractRepository(BaseContext context)
        {
            _context = context;
        }


        #region ADD
        public virtual T Add(T entity)
        {
            entity = Table().Add(entity);
            return entity;
        }
        #endregion

        #region UPDATE
        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region REMOVE
        public virtual void Remove(long id)
        {
            DbSet<T> table = _context.Set<T>();
            var entity = Table().FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }
        #endregion

        #region GET
        public virtual IQueryable<T> GetAll()
        {
            return Table();
        }

        public virtual T GetById(long id)
        {
            return Table().FirstOrDefault(e => e.Id == id);
        }
        #endregion

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        private DbSet<T> Table()
        {
            return _context.Set<T>();
        }
    }
}
