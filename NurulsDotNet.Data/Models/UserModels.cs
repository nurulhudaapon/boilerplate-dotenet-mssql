using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurulsDotNet.Data.Models
{
  public class User : BaseResourceModel
  {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? OrgId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public UserType Type { get; set; }
    public DateTime? LastLoginAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AuthToken { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RecoveryToken { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Password { get; set; }

    [JsonIgnore]
    public bool IsValidIdentifier => !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Email);

    [JsonIgnore]
    public bool IsValidCredentials => !string.IsNullOrEmpty(Password);

    [JsonIgnore]
    public bool IsValidForAuth => IsValidIdentifier && IsValidCredentials;
  }

  public class UserAuthRequest
  {
    public string Username { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// Password of the user
    /// </summary>
    [Required]
    public string Password { get; set; }

    [JsonIgnore]
    public bool IsValidIdentifier => !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Email);

    [JsonIgnore]
    public bool IsValidCredentials => !string.IsNullOrEmpty(Password);

    [JsonIgnore]
    public bool IsValidForAuth => IsValidIdentifier && IsValidCredentials;
  }

  public class UserAuthResult : User
  {
    public new string AuthToken { get; set; }
  }

  public class UserRecoverRequest
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string RecoveryToken { get; set; }
  }
  public class UserFilter : BaseFilterModel
  {
    public string[] Usernames { get; set; }
    public string[] Emails { get; set; }
    public string[] Phones { get; set; }
    public string[] Types { get; set; }

    [JsonIgnore]
    public string Password { get; set; }

    [JsonIgnore]
    public string RecoveryToken { get; set; }
  }

  /// <summary>
  /// Type of the user
  /// </summary>
  public enum UserType
  {
    /// <summary>
    /// Org User
    /// </summary>
    OrgUser,

    /// <summary>
    /// Org Admin
    /// </summary>
    OrgAdmin,

    /// <summary>
    /// Super User
    /// </summary>
    SuperUser,

    /// <summary>
    /// Super Admin
    /// </summary>
    SuperAdmin
  }
}
