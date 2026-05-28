using BankingApi.API.Models;

namespace BankingApi.API.Interfaces;

public interface IAccountService
{
    Task<List<Account>> GetAccountsAsync();
    Task<Account?> GetAccountAsync(int id);
    Task<Account> CreateAccountAsync(Account account);
}