using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Data.Models.Configs;
using NurulsDotNet.Services.UserServices;

namespace NurulsDotNet.Api.Middlewares
{
  /// <summary>
  /// JWT Middleware
  /// </summary>
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly AppConfig _appConfig;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="next"></param>
    /// <param name="appConfig"></param>
    public JwtMiddleware(RequestDelegate next, IOptions<AppConfig> appConfig)
    {
      _next = next;
      _appConfig = appConfig.Value;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userService"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context, IUserService userService)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      var userId = userService.ValidateAuthToken(token);
      if (userId != null)
      {
        // attach user to context on successful jwt validation
        context.Items[nameof(User)] = await userService.GetById(userId.Value);
      }

      await _next(context);
    }
  }
}
