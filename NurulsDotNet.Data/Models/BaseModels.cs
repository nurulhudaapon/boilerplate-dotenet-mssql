using System;
using System.Text.Json.Serialization;

namespace NurulsDotNet.Data.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class BaseResourceModel
  {
    /// <summary>
    /// ID of the resource
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Public ID of the resource
    /// </summary>
    public Guid PublicId { get; set; }
    /// <summary>
    /// Date the resource was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date the resource was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class BaseOrgResourceModel : BaseResourceModel
  {
    /// <summary>
    /// Org Id the resource belongs to
    /// </summary>
    public int OrgId { get; set; }
    /// <summary>
    /// User Id the resource belongs to
    /// </summary>
    public int UserId { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class BaseFilterModel
  {
    /// <summary>
    /// List of ID to filter the resource with
    /// </summary>
    public int[] Ids { get; set; }
    /// <summary>
    /// List of ID to filter the resource with
    /// </summary>
    public Guid[] PublicIds { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class BaseOrgResourceFilterModel : BaseFilterModel
  {
    /// <summary>
    /// List of User Id
    /// </summary>
    public int?[] UserIds { get; set; } = null;

    /// <summary>
    /// List of Org Id
    /// </summary>
    public int[] OrgIds { get; set; } = null;
  }
}