using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Data.Queries
{
  public interface IUserQueries : IBaseQueries<User, UserFilter>
  {
  }

  public class UserQueries : IUserQueries
  {
    private readonly IDbConnectionFactory _connection;
    public UserQueries(IDbConnectionFactory connection)
    {
      _connection = connection;
    }
    public async Task<User> Create(User data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          INSERT INTO user (username, email, phone, type, password, recovery_token)
          OUTPUT INSERTED.id, INSERTED.public_id, INSERTED.username, INSERTED.email, INSERTED.phone, INSERTED.type, INSERTED.created_at
          VALUES (
            @username, 
            @email, 
            @phone, 
            @type,
            IIF(@password IS NULL, NULL, HashBytes('SHA2_256', @password)),
            IIF(@recoveryToken IS NULL, NULL, HashBytes('SHA2_256', @recoveryToken))
          );
          ";
      return connection.QuerySingle<User>(sql, data);
    }

    public async Task<IEnumerable<User>> Get(UserFilter filter)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          SELECT id, public_id, phone, username, email, type, updated_at, created_at, last_login_at
          FROM user
          WHERE 1=1
            AND (@usernames IS NULL OR username IN @usernames)
            AND (@emails IS NULL OR email IN @emails)
            AND (@phones IS NULL OR phone IN @phones)
            AND (@password IS NULL OR password = HashBytes('SHA2_256', @password))
            AND (@recoveryToken IS NULL OR recovery_token = HashBytes('SHA2_256', @recoveryToken))
            AND (@types IS NULL OR type IN @types)

            -- Base Filters
            AND (@ids IS NULL OR id IN @ids)
            AND (@publicIds IS NULL OR public_id IN @publicIds)
          ";
      return await connection.QueryAsync<User>(sql, filter);
    }
    public async Task<User> Update(User data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          UPDATE user
          SET 
            updated_at = IIF(@lastLoginAt IS NULL, GETUTCDATE(), updated_at),
            
            last_login_at = @lastLoginAt,
            username = @username,
            email = @email,
            phone = @phone,
            type = @type,
            password = IIF(@password IS NULL, password, HashBytes('SHA2_256', @password)),
            recovery_token = IIF(@recoveryToken IS NULL, recovery_token, HashBytes('SHA2_256', @recoveryToken))
          OUTPUT INSERTED.id, INSERTED.public_id, INSERTED.username, INSERTED.email, INSERTED.type, INSERTED.created_at, INSERTED.updated_at, INSERTED.phone
          WHERE 1=1
            AND (@publicId IS NOT NULL OR @id IS NOT NULL)
            AND (@id IS NULL OR id = @id)
            AND (@publicId IS NULL OR public_id = @publicId)
          ;
          ";
      return connection.QuerySingle<User>(sql, data);
    }

    public async Task<User> Delete(User data)
    {
      await using var connection = _connection.GetConnection();
      DefaultTypeMap.MatchNamesWithUnderscores = true;
      var sql = @"
          DELETE FROM user
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
