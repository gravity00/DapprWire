
namespace DapprWire;

/// <summary>
/// Provides extension methods for <see cref="IDatabaseSqlRunner"/> instances.
/// </summary>
public static partial class DatabaseSqlRunnerExtensions
{
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    extension(IDatabaseSqlRunner databaseSqlRunner)
    {
        private void EnsureNotNull()
        {
            if (databaseSqlRunner is null)
                throw new ArgumentNullException(nameof(databaseSqlRunner));
        }
    }
}