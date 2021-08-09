using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Data.Queries
{
  public interface IApiKeyQueries : IBaseQueries<ApiKey, ApiKeyFilter>
  {
  }

  public class ApiKeyQueries : IApiKeyQueries
  {
    private readonly IDbConnectionFactory _connection;
    public ApiKeyQueries(IDbConnectionFactory connection)
    {
      _connection = connection;
    }
    public async Task<ApiKey> Create(ApiKey data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          INSERT INTO [main].[api_key] (name, status, user_id, org_id, key)
          OUTPUT INSERTED.*
          VALUES (
            @name, 
            @status, 
            @userId, 
            @orgId,
            @key
          );
          ";
      return connection.QuerySingle<ApiKey>(sql, data);
    }

    public async Task<IEnumerable<ApiKey>> Get(ApiKeyFilter filter)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          SELECT *
          FROM [main].[api_key]
          WHERE 1=1
            AND (@key IS NULL OR [key] = @key)
            AND (@names IS NULL OR name IN @names)
            AND (@userIds IS NULL OR user_id IN @userIds)
            AND (@orgIds IS NULL OR org_id IN @orgIds)
            AND (@statuses IS NULL OR status IN @statuses)

            -- Base Filters
            AND (@ids IS NULL OR id IN @ids)
            AND (@publicIds IS NULL OR public_id IN @publicIds);
          ";
      return await connection.QueryAsync<ApiKey>(sql, filter);
    }
    public async Task<ApiKey> Update(ApiKey data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          UPDATE [main].[api_key]
          SET 
            [updated_at] = GETUTCDATE(),
            [name] = @name,
            [status] = @status,
            [user_id] = @userId,
            [org_id] = @orgId,
            [key] = @key
          OUTPUT INSERTED.*
          WHERE 1=1
            AND (@publicId IS NOT NULL OR @id IS NOT NULL)
            AND (@id IS NULL OR id = @id)
            AND (@publicId IS NULL OR public_id = @publicId)
          ;
          ";
      return connection.QuerySingle<ApiKey>(sql, data);
    }
    public async Task<ApiKey> Delete(ApiKey data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          DELETE FROM [main].[api_key]
          WHERE 1=1
            AND (@publicId IS NOT NULL OR @id IS NOT NULL)
            AND (@id IS NULL OR id = @id)
            AND (@publicId IS NULL OR public_id = @publicId)
          ;
          ";
      await connection.QueryAsync(sql, data);
      return data;
    }
  }
}
