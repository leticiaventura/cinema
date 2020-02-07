using System.Linq;

namespace Cinema.Domain.Common
{
    public interface IService<T> where T : Entity
    {
        T Add(AbstractAddCommand<T> command);
        bool Update(AbstractUpdateCommand<T> command);
        bool Remove(long id);
        T GetById(long id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(int quantity);
    }
}
