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
}