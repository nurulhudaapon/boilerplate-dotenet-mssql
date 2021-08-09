using System.Threading.Tasks;
using NurulsDotNet.Api.Services;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Services.UserServices
{
  public interface IUserService : IBaseService<User, UserFilter>
  {
    User GetCurrent();
    Task<User> Authenticate(User user);
    Task<UserAuthResult> Authenticate(UserAuthRequest model);
    int? ValidateAuthToken(string token);
  }
}