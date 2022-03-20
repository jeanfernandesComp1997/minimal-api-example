using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data;
using Minimal.Api.Models;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseMySql(connection, serverVersion));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/customer", async (
    MinimalContextDb context) =>
    {
        if (context?.Customers == null) throw new ApplicationException();

        return Results.Ok(await context.Customers.ToListAsync());
    })
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
            : Results.BadRequest();
    })
    .Produces<Customer>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostCustomer")
    .WithTags("Customer");

app.Run();