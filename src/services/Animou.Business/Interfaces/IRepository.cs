using Animou.Business.Models;
using System.Linq.Expressions;

namespace Animou.Business.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);
        Task DeleteFromQuery(Expression<Func<T, bool>> predicate);
        Task<T?> GetById(Guid? id);
        Task<List<T>> GetAll();
        Task<IEnumerable<T>> GetCustomized(Expression<Func<T, bool>> predicate);
    }

    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
