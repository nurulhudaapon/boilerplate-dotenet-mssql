using System;
using System.Text.Json.Serialization;

namespace NurulsDotNet.Data.Models
{
  /// <summary>
  /// Org Model
  /// </summary>
  public class Org : BaseResourceModel
  {
    /// <summary>
    /// Name of the Org
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Unique Subdomain to be used for the org
    /// </summary>
    public string Subdomain { get; set; }

    /// <summary>
    /// Status of the Org
    /// </summary>
    public OrgStatus Status { get; set; }

    /// <summary>
    /// User ID of the Org Owner
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Active Theme of the Org
    /// </summary>
    public int? ThemeId { get; set; }
  }

  /// <summary>
  /// Org Status
  /// </summary>
  public enum OrgStatus
  {
    /// <summary>
    /// Active Org
    /// </summary>
    Active,

    /// <summary>
    /// Inactive Org
    /// </summary>
    Inactive,
  }
  /// <summary>
  /// Org Filter
  /// </summary>
  public class OrgFilter : BaseFilterModel
  {
    /// <summary>
    /// List of name
    /// </summary>
    public string[] Names { get; set; }

    /// <summary>
    /// List of subdomain
    /// </summary>
    public string[] Subdomains { get; set; }

    /// <summary>
    /// List of User Id
    /// </summary>
    public int[] UserIds { get; set; }

    /// <summary>
    /// List of Status
    /// </summary>
    public OrgStatus[] Statuses { get; set; }
  }
}