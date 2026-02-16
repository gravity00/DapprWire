using System.Data;
using System.Data.Common;

namespace DapprWire;

public static partial class DatabaseSqlRunnerExtensions
{
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    extension(IDatabaseSqlRunner databaseSqlRunner)
    {
        #region Execute

        /// <summary>
        /// Executes a SQL command with the specified options.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the number of rows affected.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<int> ExecuteAsync(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteAsync(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command with the specified options.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int Execute(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.Execute(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command with the specified options.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the number of rows affected.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<int> ExecuteAsync(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteAsync(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command with the specified options.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public int Execute(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.Execute(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a SQL command with the specified parameters and options,
        /// that returns a single result of type T.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> ExecuteScalarAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteScalarAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command with the specified parameters and options,
        /// that returns a single result of type T.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? ExecuteScalar<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteScalar<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command with the specified parameters and options,
        /// that returns a single result of type T.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> ExecuteScalarAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteScalarAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command with the specified parameters and options,
        /// that returns a single result of type T.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? ExecuteScalar<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteScalar<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes a SQL command and returns a data reader for the results.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the data reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<DbDataReader> ExecuteReaderAsync(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteReaderAsync(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a data reader for the results.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The data reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IDataReader ExecuteReader(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteReader(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns a data reader for the results.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the data reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<DbDataReader> ExecuteReaderAsync(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteReaderAsync(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a data reader for the results.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>A task to be awaited for the data reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IDataReader ExecuteReader(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.ExecuteReader(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion
    }
}