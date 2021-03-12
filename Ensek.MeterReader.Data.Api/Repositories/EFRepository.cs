using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Repository
{
    public class EfRepository<T> : IRepository<T>
    where T : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> dbSet;
        private bool disposed;

        public EfRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

        public void Delete(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public async Task<T> Get(int id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return this.dbSet.AsQueryable();
        }

        public async Task<T> Add(T entity)
        {
            await this.dbSet.AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            this.dbSet.Update(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await this.GetAll().AnyAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            this.dbSet.RemoveRange(entities);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            this.dbSet.AddRange(entities);
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await this.context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.context?.Dispose();
            }

            this.disposed = true;
        }
    }
}
