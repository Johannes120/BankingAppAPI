/// <summary>
/// Application Startup Configuration and Dependency Injection
/// feat: configure dependency injection and application startup
/// 
/// - Register OpenAPI and Swagger documentation services
/// - Configure SQLite database with ApplicationDbContext
/// - Register AccountService as scoped dependency
/// - Enable JSON serialization with circular reference handling
/// - Implement database auto-migration on application startup
/// - Configure API endpoints and middleware pipeline
/// </summary>

using BankingApi.API.Data;
using BankingApi.API.Interfaces;
using BankingApi.API.Models;
using BankingApi.API.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Auto-create and update database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    message = "Banking API is running.",
    endpoints = new[] { "/customers", "/accounts", "/health", "/swagger" }
}))
    .WithName("Root");

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
    .WithName("Health");

app.MapGet("/accounts", async (IAccountService service) =>
    await service.GetAccountsAsync())
    .WithName("GetAccounts");

app.MapGet("/accounts/{id}", async (int id, IAccountService service) =>
{
    var account = await service.GetAccountAsync(id);
    return account is not null ? Results.Ok(account) : Results.NotFound();
})
    .WithName("GetAccountById");

app.MapPost("/accounts", async (Account account, IAccountService service) =>
{
    var validationResult = ValidateModel(account);
    if (validationResult is not null)
    {
        return validationResult;
    }

    try
    {
        var createdAccount = await service.CreateAccountAsync(account);
        return Results.Created($"/accounts/{createdAccount.Id}", createdAccount);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPut("/accounts/{id}", async (int id, Account account, IAccountService service) =>
{
    var validationResult = ValidateModel(account);
    if (validationResult is not null)
    {
        return validationResult;
    }

    try
    {
        var updatedAccount = await service.UpdateAccountAsync(id, account);
        return updatedAccount is not null ? Results.Ok(updatedAccount) : Results.NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("UpdateAccount");

app.MapDelete("/accounts/{id}", async (int id, IAccountService service) =>
{
    var deleted = await service.DeleteAccountAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
    .WithName("DeleteAccount");


app.MapGet("/customers", async (ApplicationDbContext context) =>
    await context.Customers.ToListAsync())
    .WithName("GetCustomers");

app.MapPost("/customers", async (Customer customer, ApplicationDbContext context) =>
{
    var validationResult = ValidateModel(customer);
    if (validationResult is not null)
    {
        return validationResult;
    }

    await context.Customers.AddAsync(customer);
    await context.SaveChangesAsync();
    return Results.Created($"/customers/{customer.Id}", customer);
})
    .WithName("CreateCustomer");

app.MapPut("/customers/{id}", async (int id, Customer customer, ApplicationDbContext context) =>
{
    var validationResult = ValidateModel(customer);
    if (validationResult is not null)
    {
        return validationResult;
    }

    var existingCustomer = await context.Customers.FindAsync(id);
    if (existingCustomer is null)
    {
        return Results.NotFound();
    }

    existingCustomer.FirstName = customer.FirstName;
    existingCustomer.LastName = customer.LastName;
    existingCustomer.Email = customer.Email;

    context.Customers.Update(existingCustomer);
    await context.SaveChangesAsync();
    return Results.Ok(existingCustomer);
})
    .WithName("UpdateCustomer");

app.MapDelete("/customers/{id}", async (int id, ApplicationDbContext context) =>
{
    var customer = await context.Customers.FindAsync(id);
    if (customer is null)
    {
        return Results.NotFound();
    }

    context.Customers.Remove(customer);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
    .WithName("DeleteCustomer");


static IResult? ValidateModel(object model)
{
    var validationContext = new ValidationContext(model);
    var validationResults = new List<ValidationResult>();

    if (Validator.TryValidateObject(model, validationContext, validationResults, true))
    {
        return null;
    }

    var errors = validationResults
        .SelectMany(r => r.MemberNames.DefaultIfEmpty(string.Empty)
            .Select(member => new { member, message = r.ErrorMessage ?? string.Empty }))
        .GroupBy(x => x.member)
        .ToDictionary(g => g.Key, g => g.Select(x => x.message).ToArray());

    return Results.ValidationProblem(errors);
}

app.Run();
