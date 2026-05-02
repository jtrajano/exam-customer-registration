using Scalar.AspNetCore;
using CustomerRegistration.Application;
using CustomerRegistration.Infrastructure;
using CustomerRegistration.Application.Interfaces;
using CustomerRegistration.Application.DTOs;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.AddCustomerEndpoints();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }

