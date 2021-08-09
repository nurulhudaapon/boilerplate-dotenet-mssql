using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using NurulsDotNet.Api.Filters;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Services.ApiKeyServices;
using NurulsDotNet.Services.OrgServices;
using NurulsDotNet.Services.UserServices;

namespace NurulsDotNet.Api.Controllers
{
  /// <summary>
  /// Root Endpoint
  /// </summary>
  [ApiController]
  [Route("")]
  public class RootController : ControllerBase
  {
    private readonly ILogger<RootController> _logger;
    private readonly IOrgService _orgService;
    private readonly IUserService _userService;
    private readonly IApiKeyService _apiKeyService;

    /// <summary>
    /// Org Controller Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userService"></param>
    /// <param name="apiKeyService"></param>
    public RootController(
      ILogger<RootController> logger,
      IOrgService service,
      IUserService userService,
      IApiKeyService apiKeyService
)
    {
      _logger = logger;
      _orgService = service;
      _userService = userService;
      _apiKeyService = apiKeyService;
    }

    /// <summary>
    /// Root Controller
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Root()
    {
      var subdomain = HttpContext.Items["subdomain"]?.ToString();
      if (!string.IsNullOrEmpty(subdomain))
      {
        var org = await _orgService.GetBySubdomain(subdomain);
        var orgResponse = new
        {
          subdomain,
          org,
          apiKeys = await _apiKeyService.GetByOrgId(org?.Id ?? 0),
        };
        return Ok(orgResponse);
      }
      var response = new
      {
        subdomain,
        context = HttpContext.Request.Host,
        orgs = await _orgService.GetAll(),
        users = await _userService.GetAll(),
        apiKeys = await _apiKeyService.GetAll(),
      };
      return Ok(response);
    }
  }
}
