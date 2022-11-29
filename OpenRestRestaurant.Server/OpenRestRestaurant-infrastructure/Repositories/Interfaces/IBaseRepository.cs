using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public Task AddAsync(TEntity entity);
        public Task AddRangeAsync (IEnumerable<TEntity> entities);
        public  void DeleteAsync(TEntity entity);
        public  void DeleteRangeAsync(IEnumerable<TEntity> entities);
        public  IQueryable<TEntity> FindByExpresion(Expression<Func<TEntity, bool>> expression);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(Guid id);
        public void UpdateAsync(TEntity entity);

    }
}

