using System.Linq.Expressions;
using backend.Filter;

namespace backend.Repository {
    public interface IRepository<T> where T : class {
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task AddAll(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> entities);
        void Update(T entity);
    }
}
