namespace DapprWire;

public static partial class DatabaseSqlRunnerExtensions
{
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    extension(IDatabaseSqlRunner databaseSqlRunner)
    {
        #region Query

        /// <summary>
        /// Executes a SQL command and returns a collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query results.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<IReadOnlyCollection<T>> QueryAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query results.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IReadOnlyCollection<T> Query<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.Query<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns a collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query results.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<IReadOnlyCollection<T>> QueryAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query results.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IReadOnlyCollection<T> Query<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.Query<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QuerySingle

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T> QuerySingleAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T QuerySingle<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingle<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T> QuerySingleAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T QuerySingle<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingle<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QuerySingleOrDefault

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> QuerySingleOrDefaultAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleOrDefaultAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? QuerySingleOrDefault<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleOrDefault<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> QuerySingleOrDefaultAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleOrDefaultAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a single result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? QuerySingleOrDefault<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QuerySingleOrDefault<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QueryFirst

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T> QueryFirstAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T QueryFirst<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirst<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T> QueryFirstAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T QueryFirst<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirst<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QueryFirstOrDefault

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstOrDefaultAsync<T>(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? QueryFirstOrDefault<T>(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstOrDefault<T>(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstOrDefaultAsync<T>(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns the first result of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The query result.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T? QueryFirstOrDefault<T>(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryFirstOrDefault<T>(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QueryMultiple

        /// <summary>
        /// Executes a SQL command and returns a grid reader for multiple result sets.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the database grid reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<IDatabaseGridReader> QueryMultipleAsync(
            string sql,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryMultipleAsync(sql, SqlOptions.None, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a grid reader for multiple result sets.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <returns>The database grid reader.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IDatabaseGridReader QueryMultiple(
            string sql
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryMultiple(sql, SqlOptions.None);
        }

        /// <summary>
        /// Executes a SQL command and returns a grid reader for multiple result sets.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the database grid reader.</returns>
        public Task<IDatabaseGridReader> QueryMultipleAsync(
            string sql,
            object parameters,
            CancellationToken ct
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryMultipleAsync(sql, new SqlOptions
            {
                Parameters = parameters
            }, ct);
        }

        /// <summary>
        /// Executes a SQL command and returns a grid reader for multiple result sets.
        /// </summary>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <returns>The database grid reader.</returns>
        public IDatabaseGridReader QueryMultiple(
            string sql,
            object parameters
        )
        {
            databaseSqlRunner.EnsureNotNull();
            return databaseSqlRunner.QueryMultiple(sql, new SqlOptions
            {
                Parameters = parameters
            });
        }

        #endregion

        #region QueryStreamed

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

        /// <summary>
        /// Executes a SQL command and returns a streamed collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The async enumerator to stream each item asynchronously.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async IAsyncEnumerable<T> QueryStreamed<T>(
            string sql,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
        )
        {
            databaseSqlRunner.EnsureNotNull();

            var sqlOptions = SqlOptions.None;
            await foreach (var item in databaseSqlRunner.QueryStreamed<T>(sql, sqlOptions, ct).ConfigureAwait(false))
                yield return item;
        }

        /// <summary>
        /// Executes a SQL command and returns a streamed collection of results of type T.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="sql">The SQL command.</param>
        /// <param name="parameters">The SQL command parameters.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The async enumerator to stream each item asynchronously.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async IAsyncEnumerable<T> QueryStreamed<T>(
            string sql,
            object parameters,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
        )
        {
            databaseSqlRunner.EnsureNotNull();

            var sqlOptions = new SqlOptions
            {
                Parameters = parameters
            };
            await foreach (var item in databaseSqlRunner.QueryStreamed<T>(sql, sqlOptions, ct))
                yield return item;
        }

#endif

        #endregion
    }
}