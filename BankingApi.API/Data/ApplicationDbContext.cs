/// <summary>
/// ApplicationDbContext - Database Context Configuration
/// feat: add Entity Framework Core database context configuration
/// 
/// - Establish ApplicationDbContext for managing Customer and Account entities
/// - Configure DbSet properties for Customers and Accounts collections
/// - Enable database abstraction layer for data access operations
/// </summary>

using Microsoft.EntityFrameworkCore;
using BankingApi.API.Models;

namespace BankingApi.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
}