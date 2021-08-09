using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Api.Filters;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Services.ApiKeyServices;

namespace NurulsDotNet.Api.Controllers
{
  /// <summary>
  /// ApiKey Endpoint
  /// </summary>
  [ApiController]
  [Route("api-keys")]
  public class ApiKeysController : ControllerBase, IBaseController<ApiKey, ApiKeyFilter>
  {
    private readonly ILogger<ApiKeysController> _logger;
    private readonly IApiKeyService _service;

    /// <summary>
    /// ApiKey Controller Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ApiKeysController(ILogger<ApiKeysController> logger, IApiKeyService service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// Get ApiKey by Filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<ApiKey>> Get([FromQuery] ApiKeyFilter filter)
    {
      return await _service.GetByFilter(filter);
    }

    /// <summary>
    /// Update ApiKey
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiKey> Update(ApiKey data)
    {
      return await _service.Update(data);
    }

    /// <summary>
    /// Patch ApiKey
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPatch]
    [Authorize(UserType.OrgAdmin)]
    public async Task<ApiKey> Patch(ApiKey data)
    {
      return await _service.Patch(data);
    }

    /// <summary>
    /// Create ApiKey
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(UserType.OrgAdmin)]
    public async Task<ApiKey> Create(ApiKey data)
    {
      var user = (User)HttpContext.Items[nameof(User)];
      data.OrgId = (int)user.OrgId;
      data.UserId = user.Id;
      return await _service.Create(data);
    }

    /// <summary>
    /// Delete ApiKey
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<ApiKey> Delete(ApiKey data)
    {
      _logger.LogInformation("Delete not possible");
      throw new NotImplementedException();
    }

    /// <summary>
    /// Get ApiKey by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(UserType.OrgAdmin)]
    public Task<ApiKey> GetById(int id)
    {
      return _service.GetById(id);
    }

    /// <summary>
    /// Get ApiKey by Public ID
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/public/api-keys/{publicId}")]
    public Task<ApiKey> GetByPublicId(Guid publicId)
    {
      return _service.GetByPublicId(publicId);

    }

    /// <summary>
    /// Delete ApiKey by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    public Task<ApiKey> DeleteById(int id)
    {
      return _service.DeleteById(id);
    }

  }
}
