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

namespace NurulsDotNet.Services.OrgServices
{
  public class OrgService : IOrgService
  {
    private readonly IOrgQueries _queries;
    private AppAuthConfig _appAuthConfig;
    public OrgService(IOrgQueries queries, IOptions<AppConfig> appConfig)
    {
      _queries = queries;
      _appAuthConfig = appConfig.Value.Auth;
    }

    public async Task<IEnumerable<Org>> GetAll()
    {
      return await _queries.Get(new OrgFilter());
    }

    public async Task<IEnumerable<Org>> GetByFilter(OrgFilter filter)
    {
      return await _queries.Get(filter);
    }

    public async Task<Org> GetById(int id)
    {
      var list = await _queries.Get(new OrgFilter { Ids = new[] { id } });
      return list.Single();
    }
    public async Task<Org> GetBySubdomain(string subdomain)
    {
      var list = await _queries.Get(new OrgFilter { Subdomains = new[] { subdomain } });
      return list.FirstOrDefault();
    }

    public async Task<Org> GetByPublicId(Guid publicId)
    {
      var list = await _queries.Get(new OrgFilter { PublicIds = new[] { publicId } });
      return list.Single();
    }

    public async Task<Org> Update(Org data)
    {
      return await _queries.Update(data);
    }

    public async Task<Org> Patch(Org data)
    {
      var existingData = await GetById(data.Id);
      data = ObjectMerger.MergeObjects<Org>(existingData, data);
      return await _queries.Update(data);
    }

    public async Task<Org> Create(Org data)
    {
      return await _queries.Create(data);
    }

    public Task<Org> DeleteById(int id)
    {
      throw new NotImplementedException();
    }

    public Task<Org> Delete(Org data)
    {
      throw new NotImplementedException();
    }
  }
}
