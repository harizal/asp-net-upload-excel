using BTPNS.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTPNS.Contracts
{
    public interface IUnitOfWork<TContext> where TContext : IDbContext
    {
        IGenericRepository<T> GetGenericRepository<T>()
           where T : class;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// the dispose method is called automatically by the injector depending on the lifestyle
        /// </summary>
        void Dispose();

        /// <summary>
        /// Saves current context changes.
        /// </summary>
        void SaveChanges();

        void RevertChanges();

        void DetachAllEntities();

        void Restart();

        IDbContext ReturnContext();
    }
}