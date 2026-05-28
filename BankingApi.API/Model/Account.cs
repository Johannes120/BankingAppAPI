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
