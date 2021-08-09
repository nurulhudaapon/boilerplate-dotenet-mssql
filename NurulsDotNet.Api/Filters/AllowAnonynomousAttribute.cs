using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NurulsDotNet.Api.Filters
{
  /// <summary>
  /// Allow Anonymous
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class AllowAnonymousAttribute : Attribute
  { }
}
