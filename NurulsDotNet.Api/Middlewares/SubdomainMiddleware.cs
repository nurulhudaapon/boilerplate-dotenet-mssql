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
  /// Subdomain  Middleware
  /// </summary>
  public class SubdomainMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly string[] _reservedSubdomains = new [] { "api", "web", "status", "3status-dev", "www" };
    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="next"></param>
    public SubdomainMiddleware(RequestDelegate next)
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
      var subdomain = GetSubDomain(context);
      context.Items["subdomain"] = subdomain;
      await _next(context);
    }

    private string GetSubDomain(HttpContext context)
    {
      var subDomain = string.Empty;

      var host = context.Request.Host.Host;

      if (!string.IsNullOrWhiteSpace(host) && host.Contains("."))
      {
        var domains = host.Split('.');
        if(domains.Length > 1 && domains[0].Length > 3) subDomain = domains[0];
        if(_reservedSubdomains.Contains(subDomain?.ToLower())) subDomain = string.Empty;
      }

      subDomain = string.IsNullOrEmpty(subDomain) ? null : subDomain.Trim().ToLower();
      return subDomain;
    }
  }
}
