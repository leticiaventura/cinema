using System.Linq;

namespace Cinema.Domain.Common
{
    public interface IRepository<T> where T : Entity
    {
        T Add(T entity);
        bool Update(T entity);
        bool Remove(long id);
        T GetById(long id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(int quantity);
    }
}
