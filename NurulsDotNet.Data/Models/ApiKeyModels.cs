using System;
using System.Text.Json.Serialization;

namespace NurulsDotNet.Data.Models
{
  /// <summary>
  /// ApiKey Model
  /// </summary>
  public class ApiKey : BaseOrgResourceModel
  {
    /// <summary>
    /// Name of the ApiKey
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Key
    /// </summary>
    public Guid? Key { get; set; }

    /// <summary>
    /// Status of the ApiKey
    /// </summary>
    public ApiKeyStatus Status { get; set; }
  }

  /// <summary>
  /// ApiKey Status
  /// </summary>
  public enum ApiKeyStatus
  {
    /// <summary>
    /// Active ApiKey
    /// </summary>
    Active,

    /// <summary>
    /// Inactive ApiKey
    /// </summary>
    Inactive,

    /// <summary>
    /// Deleted ApiKey
    /// </summary>
    Deleted,
  }
  /// <summary>
  /// ApiKey Filter
  /// </summary>
  public class ApiKeyFilter : BaseOrgResourceFilterModel
  {
    /// <summary>
    /// Private Key
    /// </summary>
    public Guid? Key { get; set; } = null;

    /// <summary>
    /// List of name
    /// </summary>
    public int?[] Names { get; set; } = null;

    /// <summary>
    /// List of Status
    /// </summary>
    public ApiKeyStatus?[] Statuses { get; set; } = null;
  }
}