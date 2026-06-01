/// <summary>
/// Account Domain Model with Validation Constraints
/// feat: add Account domain model with validation constraints
/// 
/// - Define Account entity with Id, AccountNumber, Balance, and CustomerId properties
/// - Implement data annotations for validation (required fields, string length, range)
/// - Establish relationship with Customer entity for account-customer association
/// </summary>

using System.ComponentModel.DataAnnotations;

namespace BankingApi.API.Models;

public class Account
{
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string AccountNumber { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }
}
