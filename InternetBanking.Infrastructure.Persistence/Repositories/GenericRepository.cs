using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _dbContext;

        public GenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<Entity> AddAsync(Entity t)
        {
            await _dbContext.Set<Entity>().AddAsync(t);
            await _dbContext.SaveChangesAsync();
            return t;
        }

        public async Task UpdateAsync(Entity t, int id)
        {
            Entity entry = await _dbContext.Set<Entity>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(t);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Entity t)
        {
            _dbContext.Set<Entity>().Remove(t);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await _dbContext.Set<Entity>().ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }

        public virtual async Task<List<Entity>> GetAllWithIncludesAsync(List<string> props)
        {
            var query = _dbContext.Set<Entity>().AsQueryable();

            foreach (string prop in props)
            {
                query = query.Include(prop);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GetByIdWithIncludeAsync(int id, List<string> props, List<string> colls)
        {
            var query = await _dbContext.Set<Entity>().FindAsync(id);

            foreach (string prop in props)
            {
                _dbContext.Entry(query).Reference(prop).Load();
            }

            foreach (string coll in colls)
            {
                _dbContext.Entry(query).Collection(coll).Load();
            }

            return query;
        }
    }
}