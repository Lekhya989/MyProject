using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ApptManager.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _db;

        public GenericRepository(IDbConnection db)
        {
            _db = db;
        }

        /// <summary>
        /// Pluralizes the entity class name. 
        /// E.g. T = User → "Users", Slot → "Slots", Booking → "Bookings"
        /// </summary>
        protected string TableName => typeof(T).Name + "s";

        public async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";
            return await _db.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> FindAsync(string sql, object? param = null)
            => await _db.QueryAsync<T>(sql, param);

        public async Task AddAsync(string sql, T entity)
            => await _db.ExecuteAsync(sql, entity);

        public async Task UpdateAsync(string sql, T entity)
            => await _db.ExecuteAsync(sql, entity);

        public async Task<int> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
