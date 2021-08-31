using BTPNS.Contracts;
using BTPNS.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace BTPNS.DAL
{
    public class SqlUnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : class, IDbContext
    {
        private IDbContext _context;

        public SqlUnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public IDbContext ReturnContext()
        {
            return _context;
        }

        public IGenericRepository<T> CreateRepoAndContext<T>()
            where T : class
        {
            return new SqlGenericRepository<T, TContext>(_context);
        }

        //load a context automatically
        // the context is disposed when this UOW obejct is disposed > at end of webrequest (simple injector weblifestyle request)

        public IGenericRepository<T> GetGenericRepository<T>()
            where T : class
        {
            return new SqlGenericRepository<T, TContext>(_context);
        }

        /// <summary>
        /// Saves context changes
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //LogEntityValidationErrors(e.Message.ToString());
            }
        }

        /// <summary>
        /// Reverts the changes.
        /// </summary>
        public void RevertChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }

        public void DetachAllEntities()
        {
            var entries = _context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// the dispose method is called automatically by the injector depending on the lifestyle
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Restart()
        {
            if (_context != null)
                _context.Dispose();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}