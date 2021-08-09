using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NurulsDotNet.Api.Filters;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Services.UserServices;

namespace NurulsDotNet.Api.Controllers
{
  /// <summary>
  /// User Endpoint
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class UsersController : ControllerBase, IBaseController<User, UserFilter>
  {
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _service;

    /// <summary>
    /// User Controller Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public UsersController(ILogger<UsersController> logger, IUserService service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// Authenticate User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("authenticate")]
    public async Task<UserAuthResult> Authenticate([FromBody] UserAuthRequest request)
    {
      return await _service.Authenticate(request);
    }

    /// <summary>
    /// Get Logged In User
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("current")]
    public User GetCurrentUser()
    {
      var user = (User)HttpContext.Items["User"];
      return user;
    }

    /// <summary>
    /// Get User by Filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize(UserType.SuperUser)]
    [HttpGet]
    public async Task<IEnumerable<User>> Get([FromQuery] UserFilter filter)
    {
      return await _service.GetByFilter(filter);
    }

    /// <summary>
    /// Update User
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<User> Update(User data)
    {
      return await _service.Update(data);
    }

    /// <summary>
    /// Patch User
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<User> Patch(User data)
    {
      return await _service.Patch(data);
    }

    /// <summary>
    /// Create User
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<User> Create(User data)
    {
      return await _service.Create(data);

    }

    /// <summary>
    /// Get User by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{id}")]
    public Task<User> GetById(int id)
    {
      return _service.GetById(id);
    }

    /// <summary>
    /// Get User by Public ID
    /// </summary>
    /// <param name="publicId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("/public/[controller]/{publicId}")]
    public Task<User> GetByPublicId(Guid publicId)
    {
      return _service.GetByPublicId(publicId);

    }

    /// <summary>
    /// Delete User by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    public Task<User> DeleteById(int id)
    {
      return _service.DeleteById(id);
    }
  }
}
