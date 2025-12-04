using System.Linq.Expressions;
namespace todolistapp.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    
        // Advanced feature – query support
        // Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }

}