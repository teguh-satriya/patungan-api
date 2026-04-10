using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.DataAccess.Repositories;
using Patungan.Services.Interfaces;
using Patungan.Services.Services;
using Patungan.Shared.Settings;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

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
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IMonthlySummaryRepository, MonthlySummaryRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Add services to the container.
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthServices>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IMonthlySummaryService, MonthlySummaryService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? "")),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings?.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutter", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "Example API with Swagger JSON generation"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFlutter");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
