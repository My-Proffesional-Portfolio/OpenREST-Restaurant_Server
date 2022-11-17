using Microsoft.EntityFrameworkCore;
using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {

        private readonly OpenRestRestaurantDbContext _dbContext;

        public BaseRepository(OpenRestRestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public virtual void DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public virtual void DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public virtual IQueryable<TEntity> FindByExpresion(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
        }
    }
}
