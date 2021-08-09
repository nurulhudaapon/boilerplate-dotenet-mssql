using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models.ExceptionModels;

namespace NurulsDotNet.Api.Middlewares
{
  /// <summary>
  /// Error Handler Middleware
  /// </summary>
  public class ErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="next"></param>
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception error)
      {
        var response = context.Response;
        response.ContentType = "application/json";
        var errors = new List<object>();

        switch (error)
        {
          case AppException e:
            // custom application error
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
          case UnauthorizedAccessException e:
            // custom application error
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            break;
          case KeyNotFoundException e:
            // not found error
            response.StatusCode = (int)HttpStatusCode.NotFound;
            break;
          default:
            // unhandled error
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }
        var newError = new
        {
          error = error?.Message,
          stackTrace = response.StatusCode == (int)HttpStatusCode.InternalServerError ? error.StackTrace : null
        };
        
        errors.Add(newError);
        var result = JsonSerializer.Serialize(errors);

        await response.WriteAsync(result);
      }
    }
  }
}
