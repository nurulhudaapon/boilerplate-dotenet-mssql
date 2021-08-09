using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurulsDotNet.Data.Models.Configs
{
  public class AppConfig
  {
    public AppAuthConfig Auth { get; set; }
  }

  public class AppAuthConfig
  {
    public string ApiKeyHeaderName { get; set; }
    public string ApiKeyQueryKeyName { get; set; }
    public string SecretKey { get; set; }
    public int ValidForDays { get; set; }
  }
}
