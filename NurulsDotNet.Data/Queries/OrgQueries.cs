using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Data.Queries
{
  public interface IOrgQueries : IBaseQueries<Org, OrgFilter>
  {
  }

  public class OrgQueries : IOrgQueries
  {
    private readonly IDbConnectionFactory _connection;
    public OrgQueries(IDbConnectionFactory connection)
    {
      _connection = connection;
    }
    public async Task<Org> Create(Org data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          INSERT INTO org (name, subdomain, status, user_id, theme_id)
          OUTPUT INSERTED.*
          VALUES (
            @name, 
            @subdomain,
            @status, 
            @userId, 
            @themeId
          );
          ";
      return connection.QuerySingle<Org>(sql, data);
    }

    public async Task<IEnumerable<Org>> Get(OrgFilter filter)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          SELECT *
          FROM org
          WHERE 1=1
            AND (@ids IS NULL OR id IN @ids)
            AND (@names IS NULL OR name IN @names)
            AND (@subdomains IS NULL OR subdomain IN @subdomains)

            -- Base Filters
            AND (@statuses IS NULL OR status IN @statuses)
            AND (@publicIds IS NULL OR public_id IN @publicIds);
          ";
      return await connection.QueryAsync<Org>(sql, filter);
    }
    public async Task<Org> Update(Org data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          UPDATE org
          SET 
            updated_at = GETUTCDATE(),
            
            name = @name,
            subdomain = @subdomain,
            status = @status,
            user_id = @userId,
            theme_id = @themeId
          OUTPUT INSERTED.*
          WHERE 1=1
            AND (@publicId IS NOT NULL OR @id IS NOT NULL)
            AND (@id IS NULL OR id = @id)
            AND (@publicId IS NULL OR public_id = @publicId)
          ;
          ";
      return connection.QuerySingle<Org>(sql, data);
    }
    public async Task<Org> Delete(Org data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          DELETE FROM org
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
