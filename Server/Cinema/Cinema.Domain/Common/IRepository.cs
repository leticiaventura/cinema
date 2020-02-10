using System.Linq;

namespace Cinema.Domain.Common
{
    public interface IRepository<T> where T : Entity
    {
        T Add(T entity);
        void Update(T entity);
        void Remove(long id);
        T GetById(long id);
        IQueryable<T> GetAll();
        bool Save();
    }
}
