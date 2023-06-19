using System.Linq.Expressions;
using backend.Data;
using backend.Filter;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository.Impl {
    public class RepositoryImpl<T, ID> : IRepository<T, ID> where T : class {

        // Will be used in sub classes so make it protected
        protected readonly DbContext _context;

        public RepositoryImpl(DbContext context) {
            _context = context;
        }

        public async Task<T> Get(ID id) {

            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<IEnumerable<T>> GetAll() {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T entity) {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddAll(IEnumerable<T> entities) {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity) {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteAll(IEnumerable<T> entities) {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity) {
            _context.Set<T>().Update(entity);
        }
    }
}
