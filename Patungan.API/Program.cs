using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.DataAccess.Repositories;
using Patungan.Services.Interfaces;
using Patungan.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PatunganDbContext>(
        optionsAction => optionsAction.UseNpgsql(builder.Configuration.GetConnectionString("PatunganDBConnection")),
        ServiceLifetime.Scoped
    );

// Register password hasher for UserModel
builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
builder.Services.AddScoped<ITransactionTypeTemplateRepository, TransactionTypeTemplateRepository>();

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthServices>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
