/// <summary>
/// IAccountService Interface - Account Management Operations
/// feat: define IAccountService interface for account management operations
/// 
/// - Create service contract for account CRUD operations
/// - Define async methods for retrieving, creating, updating, and deleting accounts
/// - Enable dependency injection and loose coupling for AccountService implementation
/// </summary>

using BankingApi.API.Models;

namespace BankingApi.API.Interfaces;

public interface IAccountService
{
    Task<List<Account>> GetAccountsAsync();
    Task<Account?> GetAccountAsync(int id);
    Task<Account> CreateAccountAsync(Account account);
    Task<Account?> UpdateAccountAsync(int id, Account account);
    Task<bool> DeleteAccountAsync(int id);
}