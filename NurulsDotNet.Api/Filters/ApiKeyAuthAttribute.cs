using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NurulsDotNet.Data.Models;
using NurulsDotNet.Data.Models.Configs;
using NurulsDotNet.Data.Models.ExceptionModels;
using NurulsDotNet.Services.ApiKeyServices;
using NurulsDotNet.Services.UserServices;

namespace Porichoy.Api.Filters
{
  /// <summary>
  /// ApiKeyAuthAttribute
  /// </summary>
  [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]

  public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
  {
    private readonly UserType[] _userTypes;
    /// <summary>
    /// Default CTOR
    /// </summary>
    public ApiKeyAuthAttribute()
    {

    }

    /// <summary>
    /// Role accepting CTOR
    /// </summary>
    /// <param name="userTypes"></param>
    public ApiKeyAuthAttribute(params UserType[] userTypes)
    {
      _userTypes = userTypes;
    }

    /// <summary>
    /// OnActionExecutingAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var appAuthConfig = context.HttpContext.RequestServices.GetRequiredService<IOptions<AppConfig>>().Value.Auth;
      var apiKeyService = context.HttpContext.RequestServices.GetRequiredService<IApiKeyService>();
      var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

      context.HttpContext.Request.Headers.TryGetValue(appAuthConfig.ApiKeyHeaderName, out var potentialApiKey);

      if (string.IsNullOrEmpty(potentialApiKey)
        && !context.HttpContext.Request.Query.TryGetValue(appAuthConfig.ApiKeyQueryKeyName, out potentialApiKey))
        throw new AppException("API Key is not provided via x-api-key header or apiKey query parameter");


      if (!Guid.TryParse(potentialApiKey, out var guidOutput))
        throw new UnauthorizedAccessException("Provided API Key is not in correct format");

      try
      {
        var apiKey = await apiKeyService.GetByKey(guidOutput);
        context.HttpContext.Items.Add(nameof(ApiKey), apiKey);

        var user = await userService.GetById(apiKey.UserId);
        context.HttpContext.Items.Add(nameof(User), user);

        // authorization
        if (_userTypes?.Length > 0 && !_userTypes.Contains(user.Type))
          context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
      }
      catch (Exception)
      {
        throw new UnauthorizedAccessException("Provided API Key is not valid");
      }
      if(context.Result == null) await next();
    }
  }
}