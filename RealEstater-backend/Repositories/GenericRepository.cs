using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : IdModel
    {
        protected RealEstaterDbContext _dbContext;
        private DbSet<T> _entities;

        public GenericRepository(RealEstaterDbContext realEstaterDbContext)
        {
            this._dbContext = realEstaterDbContext;
            this._entities = this._dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual T GetById(int id)
        {
            return _entities.SingleOrDefault(s => s.Id == id);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public T FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public void Remove(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Deleted;
        }
    }
}
