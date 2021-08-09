using System;
using System.Threading.Tasks;
using NurulsDotNet.Api.Services;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Services.ApiKeyServices
{
  public interface IApiKeyService : IBaseService<ApiKey, ApiKeyFilter>
  {
    Task<ApiKey> GetByOrgId(int orgId);
    Task<ApiKey> GetByKey(Guid key);
  }
}