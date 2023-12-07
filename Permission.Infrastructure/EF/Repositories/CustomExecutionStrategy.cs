using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.EF.Repositories
{
    public class CustomExecutionStrategy : SqlServerRetryingExecutionStrategy
    {
        public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies) : base(dependencies)
        {
        }
        public CustomExecutionStrategy(DbContext context, int maxRetryCount) : base(context, maxRetryCount)
        {
        }

        public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay, List<int> errorNumbersToAdd) :
            base(dependencies, maxRetryCount, maxRetryDelay, errorNumbersToAdd)
        {
        }

        protected override bool ShouldRetryOn(Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            SqlException? sqlException;
            if ((sqlException = ex as SqlException) != null)
            {
                return sqlException.IsTransient;
            }

            return false; // Don't retry for non-transient exceptions.
        }
    }
}
