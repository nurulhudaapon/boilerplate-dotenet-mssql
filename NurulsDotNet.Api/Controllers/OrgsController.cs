using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Api.Filters;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Services.OrgServices;

namespace NurulsDotNet.Api.Controllers
{
  /// <summary>
  /// Org Endpoint
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class OrgsController : ControllerBase, IBaseController<Org, OrgFilter>
  {
    private readonly ILogger<OrgsController> _logger;
    private readonly IOrgService _service;

    /// <summary>
    /// Org Controller Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OrgsController(ILogger<OrgsController> logger, IOrgService service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// Get Org by Filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<Org>> Get([FromQuery] OrgFilter filter)
    {
      return await _service.GetByFilter(filter);
    }

    /// <summary>
    /// Update Org
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<Org> Update(Org data)
    {
      return await _service.Update(data);
    }

    /// <summary>
    /// Patch Org
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<Org> Patch(Org data)
    {
      return await _service.Patch(data);
    }

    /// <summary>
    /// Create Org
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Org> Create(Org data)
    {
      var user = (User)HttpContext.Items[nameof(User)];
      data.UserId = user.Id;
      return await _service.Create(data);
    }

    /// <summary>
    /// Delete Org
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<Org> Delete(Org data)
    {
      _logger.LogInformation("Delete not possible");
      throw new NotImplementedException();
    }

    /// <summary>
    /// Get Org by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{id}")]
    public Task<Org> GetById(int id)
    {
      return _service.GetById(id);
    }

    /// <summary>
    /// Get Org by Public ID
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    // [Authorize]
    [HttpGet("/public/[controller]/{publicId}")]
    public Task<Org> GetByPublicId(Guid publicId)
    {
      return _service.GetByPublicId(publicId);

    }

    /// <summary>
    /// Delete Org by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    public Task<Org> DeleteById(int id)
    {
      return _service.DeleteById(id);
    }

  }
}
