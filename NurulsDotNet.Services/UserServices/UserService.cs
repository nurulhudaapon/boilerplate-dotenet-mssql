using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Data.Models.Configs;
using NurulsDotNet.Data.Models.ExceptionModels;
using NurulsDotNet.Data.Queries;
using NurulsDotNet.Utils;

namespace NurulsDotNet.Services.UserServices
{
  public class UserService : IUserService
  {
    private readonly IUserQueries _queries;
    private AppAuthConfig _appAuthConfig;
    public UserService(IUserQueries queries, IOptions<AppConfig> appConfig)
    {
      _queries = queries;
      _appAuthConfig = appConfig.Value.Auth;
    }

    public async Task<User> Authenticate(User model)
    {
      var user = await GetForAuth(model);

      // validate
      if (user == null) throw new AppException("Username or password is incorrect");

      // authentication successful 
      user.AuthToken = GenerateAuthToken(user);

      // update last login at datetime
      user.LastLoginAt = DateTime.UtcNow;
      await Update(user);

      return user;
    }

    public async Task<UserAuthResult> Authenticate(UserAuthRequest model)
    {
      var user = Mapping.Mapper.Map<User>(model);
      user = await Authenticate(user);
      return Mapping.Mapper.Map<UserAuthResult>(user);
    }


    public User GetCurrent()
    {
      return new User(); //TODO: Get from JWT Token
    }

    private async Task<User> GetForAuth(User model)
    {
      if (!model.IsValidForAuth) throw new AppException("Username or email and password required");

      var filter = new UserFilter
      {
        Password = model.Password,
        RecoveryToken = model.RecoveryToken
      };
      var isEmail = !string.IsNullOrEmpty(model.Email);
      if (isEmail)
        filter.Emails = new string[] { model.Email };
      else
        filter.Usernames = new string[] { model.Username };

      var user = await GetByFilter(filter);
      return user.FirstOrDefault();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      return await _queries.Get(new UserFilter());
    }

    public async Task<IEnumerable<User>> GetByFilter(UserFilter filter)
    {
      return await _queries.Get(filter);
    }

    public async Task<User> GetById(int id)
    {
      var list = await _queries.Get(new UserFilter { Ids = new[] { id } });
      return list.Single();
    }

    public async Task<User> GetByEmailOrUsername(string email = null, string username = null)
    {
      var list = await _queries.Get(new UserFilter { Emails = new[] { email }, Usernames = new[] { username } });
      return list.Single();
    }

    public async Task<User> GetByPublicId(Guid publicId)
    {
      var list = await _queries.Get(new UserFilter { PublicIds = new[] { publicId } });
      return list.Single();
    }

    public async Task<User> Update(User data)
    {
      return await _queries.Update(data);
    }

    public async Task<User> Patch(User data)
    {
      var existingData = await GetById(data.Id);
      data = ObjectMerger.MergeObjects<User>(existingData, data);
      return await _queries.Update(data);
    }

    public async Task<User> Create(User data)
    {
      return await _queries.Create(data);
    }

    private string GenerateAuthToken(User user)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appAuthConfig.SecretKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] {
          new Claim(nameof(User.Id), user.Id.ToString()),
          new Claim(nameof(User.Type), ((int)user.Type).ToString()),
          new Claim(nameof(User.OrgId), user?.OrgId?.ToString() ?? "0"),
          }),
        Expires = DateTime.UtcNow.AddDays(_appAuthConfig.ValidForDays),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public int? ValidateAuthToken(string token)
    {
      if (token == null)
        return null;

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appAuthConfig.SecretKey);
      try
      {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == nameof(User.Id)).Value);

        // return user id from JWT token if validation successful
        return userId;
      }
      catch (Exception)
      {
        // return null if validation fails
        return null;
      }
    }

    public Task<User> DeleteById(int id)
    {
      throw new NotImplementedException();
    }

    public Task<User> Delete(User data)
    {
      throw new NotImplementedException();
    }
  }
}
