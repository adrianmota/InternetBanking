using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity>
        where Entity : class
    {
        Task<Entity> AddAsync(Entity t);
        Task UpdateAsync(Entity t, int id);
        Task DeleteAsync(Entity t);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllWithIncludesAsync(List<string> props);
        Task<Entity> GetByIdWithIncludeAsync(int id, List<string> props, List<string> colls);
    }
}