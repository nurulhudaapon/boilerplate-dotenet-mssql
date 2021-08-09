using System.Threading.Tasks;
using NurulsDotNet.Api.Services;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Services.OrgServices
{
  public interface IOrgService : IBaseService<Org, OrgFilter>
  {
    Task<Org> GetBySubdomain(string subdomain);
  }
}