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
