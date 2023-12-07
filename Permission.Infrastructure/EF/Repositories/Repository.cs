using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Permission.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.EF.Repositories
{
    public class Repository : IRepository
    {
        private readonly PermissionDbContext db;

        public Repository(PermissionDbContext db)
        {
            this.db = db ?? throw new ArgumentException($"{nameof(PermissionDbContext)} is null");
        }

        /// <inheritdoc/>
        public async Task AddAsync<T>(T entity) where T : class
        {
            _ = entity ?? throw new ArgumentException($"Entity is null");

            await db.Set<T>().AddAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public IQueryable<T> Query<T>(CancellationToken cancellationToken = default) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            return db.Set<T>().AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<T> Find<T>(CancellationToken cancellationToken = default) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            return db.Set<T>();
        }

        /// <inheritdoc/>
        public void Remove<T>(T entity) where T : class
        {
            _ = entity ?? throw new ArgumentException($"Entity is null");

            db.Set<T>().Remove(entity);
        }

        /// <inheritdoc/>
        public IQueryable<T> CallSP<T>(string spName) where T : class
        {
            return db.Set<T>().FromSqlRaw($"EXEC {spName}").AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<int> CallSP(string spName, params SqlParameter[] sqlParameters)
        {
            return await db.Database.ExecuteSqlRawAsync(
               $"EXEC {spName} {string.Join(", ", sqlParameters.Select(x => x.ParameterName))}", sqlParameters);
        }
    }
}
