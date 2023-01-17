using Animou.Business.Interfaces;
using Animou.Business.Models;
using Animou.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animou.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly AnimouContext _context;
        protected readonly DbSet<T> DbSet;

        protected Repository(AnimouContext context)
        {
            _context = context;
            DbSet = context.Set<T>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(T entity) => DbSet.Add(entity);
        
        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(Guid id) => DbSet.Remove(new T { Id = id });

        public Task DeleteFromQuery(Expression<Func<T, bool>> expression) =>
            DbSet.AsNoTracking().Where(expression).DeleteFromQueryAsync();

        public async Task<List<T>> GetAll() => await DbSet.ToListAsync();

        public async Task<T?> GetById(Guid? id) => 
            await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<T>> GetCustomized(Expression<Func<T, bool>> predicate) =>
            await DbSet.AsNoTracking().Where(predicate).ToListAsync();

        public void Dispose() => _context?.Dispose();
    }
}
