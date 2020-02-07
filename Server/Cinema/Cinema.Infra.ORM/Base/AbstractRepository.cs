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
            _context.SaveChanges();
            return entity;
        }
        #endregion

        #region UPDATE
        public virtual bool Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }
        #endregion

        #region REMOVE
        public virtual bool Remove(long id)
        {
            DbSet<T> table = _context.Set<T>();
            var entity = Table().FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            return _context.SaveChanges() > 0;
        }
        #endregion

        #region GET
        public virtual IQueryable<T> GetAll(int size)
        {
            return Table().Take(size);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Table();
        }

        public virtual T GetById(long id)
        {
            return Table().FirstOrDefault(e => e.Id == id);
        }
        #endregion

        private DbSet<T> Table()
        {
            return _context.Set<T>();
        }
    }
}
