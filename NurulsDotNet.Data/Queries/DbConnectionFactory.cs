using Npgsql;
using NurulsDotNet.Data.Models.Configs;

namespace NurulsDotNet.Data.Queries
{
  /// <summary>
  /// DB connection factory
  /// </summary>
  public interface IDbConnectionFactory
  {
    /// <summary>
    /// Get connection
    /// </summary>
    /// <returns></returns>
    NpgsqlConnection GetConnection();

    /// <summary>
    /// Get read only connection
    /// </summary>
    /// <returns></returns>
    NpgsqlConnection GetReadOnlyConnection();
  }

  /// <summary>
  /// DB connection factory
  /// </summary>
  public class DbConnectionFactory : IDbConnectionFactory
  {
    private readonly string _connectionString;
    private readonly string _readOnlyConnectionString;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="config"></param>
    public DbConnectionFactory(DatabaseConfig config)
    {
      _connectionString = config.MainDatabaseConnectionString;
      _readOnlyConnectionString = config.ReadOnlyDatabaseConnectionString;
    }

    /// <summary>
    /// Get Read/Write Connection
    /// </summary>
    /// <returns></returns>
    public NpgsqlConnection GetConnection()
    {
      return new NpgsqlConnection(_connectionString);
    }

    /// <summary>
    /// Get Readonly Connection
    /// </summary>
    /// <returns></returns>
    public NpgsqlConnection GetReadOnlyConnection()
    {
      return new NpgsqlConnection(_readOnlyConnectionString);
    }
  }
}

