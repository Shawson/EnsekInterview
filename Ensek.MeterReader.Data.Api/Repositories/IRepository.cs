using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Repository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IQueryable<T> GetAll();

        Task<T> Get(int id);

        Task<T> Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();

        Task<bool> Any(Expression<Func<T, bool>> predicate);

        void DeleteRange(IEnumerable<T> entities);

        void AddRange(IEnumerable<T> entities);

        Task<IDbContextTransaction> BeginTransaction();
    }
}
