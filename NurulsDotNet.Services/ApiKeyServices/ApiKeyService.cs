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

namespace NurulsDotNet.Services.ApiKeyServices
{
  public class ApiKeyService : IApiKeyService
  {
    private readonly IApiKeyQueries _queries;
    private AppAuthConfig _appAuthConfig;
    public ApiKeyService(IApiKeyQueries queries, IOptions<AppConfig> appConfig)
    {
      _queries = queries;
      _appAuthConfig = appConfig.Value.Auth;
    }

    public async Task<IEnumerable<ApiKey>> GetAll()
    {
      return await _queries.Get(new ApiKeyFilter());
    }

    public async Task<IEnumerable<ApiKey>> GetByFilter(ApiKeyFilter filter)
    {
      return await _queries.Get(filter);
    }

    public async Task<ApiKey> GetById(int id)
    {
      var list = await _queries.Get(new ApiKeyFilter { Ids = new[] { id } });
      return list.Single();
    }
    public async Task<ApiKey> GetByOrgId(int orgId)
    {
      var list = await _queries.Get(new ApiKeyFilter { OrgIds = new[] { orgId } });
      return list.FirstOrDefault();
    }

    public async Task<ApiKey> GetByPublicId(Guid publicId)
    {
      var list = await _queries.Get(new ApiKeyFilter { PublicIds = new[] { publicId } });
      return list.Single();
    }
    public async Task<ApiKey> GetByKey(Guid key)
    {
      var list = await _queries.Get(new ApiKeyFilter { Key = key });
      return list.Single();
    }

    public async Task<ApiKey> Update(ApiKey data)
    {
      return await _queries.Update(data);
    }

    public async Task<ApiKey> Patch(ApiKey data)
    {
      var existingData = await GetById(data.Id);
      data = ObjectMerger.MergeObjects<ApiKey>(existingData, data);
      return await _queries.Update(data);
    }

    public async Task<ApiKey> Create(ApiKey data)
    {
      return await _queries.Create(data);
    }

    public Task<ApiKey> DeleteById(int id)
    {
      throw new NotImplementedException();
    }

    public Task<ApiKey> Delete(ApiKey data)
    {
      throw new NotImplementedException();
    }
  }
}
