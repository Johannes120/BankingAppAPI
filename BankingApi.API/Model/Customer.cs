/// <summary>
/// Customer Domain Model with Validation and Relationships
/// feat: add Customer domain model with validation and relationships
/// 
/// - Define Customer entity with Id, FirstName, LastName, and Email properties
/// - Implement email validation and string length constraints
/// - Establish one-to-many relationship with Account entity
/// - Add JsonIgnore attribute to prevent circular reference serialization
/// </summary>

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankingApi.API.Models;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Account> Accounts { get; set; } = new();
}
