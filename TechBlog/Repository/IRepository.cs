using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TechBlog.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> AddAsync(TEntity entity);
        Task<int> SaveChangesAsync();
        Task<int> DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> ExecuteFunctionAsync(string functionName, params ObjectParameter[] parameters);
        Task<int> ExecuteStoreQueryAsync(string commandText, params object[] parameters);
        bool StoredProcedureExists(string name);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate);
        int Add(TEntity entity);
        int SaveChanges();
        int Delete(TEntity entity);
        IEnumerable<TEntity> GetWithWhereClause(Expression<Func<TEntity, bool>> expr);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        int ExecuteFunction(string functionName, params ObjectParameter[] parameters);
        ObjectResult<T> ExecuteFunction<T>(string functionName, params ObjectParameter[] parameters);
        void ExecuteStoreCommand(string commandName, params ObjectParameter[] parameters);
        ObjectResult<T> ExecuteStoreQuery<T>(string commandText, params ObjectParameter[] parameters);
        IEnumerable<T> ExecuteStoredProcedure<T>(string sprocName, params object[] objectParameters);
        void Dispose();

    }
}