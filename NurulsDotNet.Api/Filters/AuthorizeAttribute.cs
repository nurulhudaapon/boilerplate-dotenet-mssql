using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Api.Filters
{
  /// <summary>
  /// Authorize attribute
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class AuthorizeAttribute : Attribute, IAuthorizationFilter
  {
    private readonly UserType[] _userTypes;
    /// <summary>
    /// Default CTOR
    /// </summary>
    public AuthorizeAttribute()
    {

    }

    /// <summary>
    /// Role accepting CTOR
    /// </summary>
    /// <param name="userTypes"></param>
    public AuthorizeAttribute(params UserType[] userTypes)
    {
      _userTypes = userTypes;
    }

    /// <summary>
    /// Authorize request
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // skip authorization if action is decorated with [AllowAnonymous] attribute
      var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
      if (allowAnonymous)
        return;

      // authentication
      var user = (User)context.HttpContext.Items[nameof(User)];
      if (user == null)
      {
        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        return;
      }
      // authorization
      if (_userTypes?.Length > 0 && !_userTypes.Contains(user.Type))
        context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
    }
  }
}
