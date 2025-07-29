using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApptManager.Repo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(string sql, object? param = null);
        Task AddAsync(string sql, T entity);
        Task UpdateAsync(string sql, T entity);
        Task<int> DeleteAsync(int id);
    }
}
