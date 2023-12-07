using Microsoft.Data.SqlClient;

namespace Permission.Domain.Interfaces
{
    public interface IRepository
    {
        /// <summary>
        /// Save entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Commit all changes
        /// </summary>
        /// <param name="priorAuthorization"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Find all readonly
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns type T</returns>
        IQueryable<T> Query<T>(CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///  Find all
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IQueryable<T> Find<T>(CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Remove<T>(T entity) where T : class;


        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <returns></returns>
        IQueryable<T> CallSP<T>(string spName) where T : class;

        /// <summary>
        /// Execute store procedure
        /// The parameters must be in the correct order
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns>affected records</returns>

        Task<int> CallSP(string spName, params SqlParameter[] parameters);
    }
}
