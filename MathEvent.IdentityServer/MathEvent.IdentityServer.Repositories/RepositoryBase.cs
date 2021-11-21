using MathEvent.IdentityServer.Contracts.Repositories;
using MathEvent.IdentityServer.Database;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MathEvent.IdentityServer.Repositories
{
    /// <summary>
    /// Базовый класс репозитория
    /// </summary>
    /// <typeparam name="T">Тип сущности, обрабатываемой в репозитории</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Контекст данных для работы с базой данных
        /// </summary>
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
    }
}
