using RealEstater_backend.Data.Models;
using System.Linq.Expressions;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : IdModel
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        bool SaveChanges();
        T FindByCondition(Expression<Func<T, bool>> predicate);
        void Remove(T obj);
    }
}
