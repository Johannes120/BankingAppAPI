/// <summary>
/// AccountService - Complete Account Management Logic Implementation
/// feat: implement AccountService with complete account management logic
/// 
/// - Implement IAccountService interface for async account operations
/// - Add GetAccountsAsync to retrieve all accounts with customer details
/// - Add GetAccountAsync for fetching individual accounts by Id
/// - Add CreateAccountAsync with customer validation and entity persistence
/// - Add UpdateAccountAsync and DeleteAccountAsync for account modifications
/// - Use Entity Framework Core for database interaction
/// </summary>

using BankingApi.API.Data;
using BankingApi.API.Interfaces;
using BankingApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.API.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _context.Accounts
            .Include(a => a.Customer)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountAsync(int id)
    {
        return await _context.Accounts
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        var customer = await _context.Customers.FindAsync(account.CustomerId);
        if (customer is null)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        account.Customer = customer;
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> UpdateAccountAsync(int id, Account account)
    {
        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount is null)
        {
            return null;
        }

        if (account.CustomerId != existingAccount.CustomerId)
        {
            var customer = await _context.Customers.FindAsync(account.CustomerId);
            if (customer is null)
            {
                throw new InvalidOperationException("Customer not found.");
            }
        }

        existingAccount.AccountNumber = account.AccountNumber;
        existingAccount.Balance = account.Balance;
        existingAccount.CustomerId = account.CustomerId;

        _context.Accounts.Update(existingAccount);
        await _context.SaveChangesAsync();
        return existingAccount;
    }

    public async Task<bool> DeleteAccountAsync(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account is null)
        {
            return false;
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }
}