using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TechBlog.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext dbContext;
        protected IObjectSet<TEntity> _objectSet;

        public Repository(DbContext context)
        {
            dbContext = context;
            _objectSet = ((IObjectContextAdapter)context).ObjectContext.CreateObjectSet<TEntity>();
        }

        #region Async

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await dbContext.Set<TEntity>().FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await dbContext.Set<TEntity>().FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().CountAsync(predicate);
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            dbContext.Set<TEntity>().Add(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            dbContext.Entry(entity).State = EntityState.Deleted;
            return await dbContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().SingleAsync(predicate);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<int> ExecuteFunctionAsync(string functionName, params ObjectParameter[] parameters)
        {
            return await dbContext.Database.ExecuteSqlCommandAsync("EXEC " + functionName, parameters);
        }

        public async Task<int> ExecuteStoreQueryAsync(string commandText, params object[] parameters)
        {
            return await dbContext.Database.ExecuteSqlCommandAsync(commandText, parameters);
        }

        #endregion

        #region sync

        public bool StoredProcedureExists(string name)
        {
            var query = dbContext.Database.SqlQuery(
                typeof(int),
                string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE [type_desc] = 'SQL_STORED_PROCEDURE' AND [name] = '{0}';", name),
                new object[] { });

            var exists = query.Cast<int>()
                .Single();

            return (exists > 0);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().Count(predicate);
        }

        public int Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            dbContext.Set<TEntity>().Add(entity);
            return dbContext.SaveChanges();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            dbContext.Entry(entity).State = EntityState.Deleted;
            return dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetWithWhereClause(Expression<Func<TEntity, bool>> expr)
        {
            return Find(expr);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext.ExecuteFunction(functionName, parameters);
        }

        public ObjectResult<T> ExecuteFunction<T>(string functionName, params ObjectParameter[] parameters)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext.ExecuteFunction<T>(functionName, parameters);
        }

        public void ExecuteStoreCommand(string commandName, params ObjectParameter[] parameters)
        {
            ((IObjectContextAdapter)dbContext).ObjectContext.ExecuteFunction(commandName, parameters);
        }

        public ObjectResult<T> ExecuteStoreQuery<T>(string commandText, params ObjectParameter[] parameters)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext.ExecuteStoreQuery<T>(commandText, parameters);
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string sprocName, params object[] objectParameters)
        {
            return dbContext.Database.SqlQuery<T>(sprocName, objectParameters);
        }

        protected virtual T ExecuteReader<T>(Func<DbDataReader, T> mapEntities,
            string exec, params object[] parameters)
        {
            using (var conn = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                using (var command = new SqlCommand(exec, conn))
                {
                    conn.Open();
                    command.Parameters.AddRange(parameters);
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            T data = mapEntities(reader);
                            return data;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (dbContext == null) return;

            dbContext.Dispose();
            dbContext = null;
        }
    }
}