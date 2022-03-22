using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data;
using Minimal.Api.Models;
using MiniValidation;
using Microsoft.AspNetCore.Identity;
using Minimal.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseMySql(connection, serverVersion));

builder.Services.AddDbContext<AppIdentityDbContext>(o =>
    o.UseMySql(connection, serverVersion));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

AuthConfiguration.AddConfiguration(builder);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/customer", async (
    MinimalContextDb context) =>
    {
        if (context?.Customers == null) throw new ApplicationException();

        return Results.Ok(await context.Customers.ToListAsync());
    })
    .Produces<List<Customer>>(StatusCodes.Status200OK)
    .WithName("GetCustomer")
    .WithTags("Customer");

app.MapGet("/customer/{id}", async (
    Guid id,
    MinimalContextDb context) =>
    {
        if (context?.Customers == null) throw new ApplicationException();

        return await context.Customers.FindAsync(id)
            is Customer customer
                ? Results.Ok(customer)
                : Results.NotFound();
    })
    .Produces<Customer>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetCustomerById")
    .WithTags("Customer");

app.MapPost("/customer", async (
    MinimalContextDb context,
    Customer customer) =>
    {
        if (!MiniValidator.TryValidate(customer, out var errors))
            return Results.ValidationProblem(errors);

        if (context?.Customers == null) throw new ApplicationException();

        context.Customers.Add(customer);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.Created($"/customer/{customer.Id}", customer)
            : Results.BadRequest("There was a problem saving the record.");
    })
    .ProducesValidationProblem()
    .Produces<Customer>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostCustomer")
    .WithTags("Customer");

app.MapPut("/customer/{id}", async (
    Guid id,
    MinimalContextDb context,
    Customer customer) =>
    {
        if (!MiniValidator.TryValidate(customer, out var errors))
            return Results.ValidationProblem(errors);

        if (context?.Customers == null) throw new ApplicationException();

        var customerQueryResult = await context.Customers.AsNoTracking<Customer>()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (customerQueryResult == null) return Results.NotFound();

        context.Customers.Update(customer);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("There was a problem updating the record.");
    })
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PutCustomer")
    .WithTags("Customer");

app.MapDelete("/customer/{id}", async (
    Guid id,
    MinimalContextDb context) =>
    {
        if (context?.Customers == null) throw new ApplicationException();

        var customerQueryResult = await context.Customers.AsNoTracking<Customer>()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (customerQueryResult == null) return Results.NotFound();

        context.Customers.Remove(customerQueryResult);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("There was a problem delete the record.");
    })
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("DeleteCustomer")
    .WithTags("Customer");

app.Run();