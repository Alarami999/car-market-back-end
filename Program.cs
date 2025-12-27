// Program.cs
// A complete single-file ASP.NET Core Web API for the car marketplace.
// This file can be run directly from the command line using `dotnet run`.

using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Settings;
using car_marketplace_api_3tier.Api.Services;
using car_marketplace_api_3tier.Data;
using car_marketplace_api_3tier.Repositories;
using car_marketplace_api_3tier.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with the CarsDB database
builder.Services.AddDbContext<CarContext>(options =>
    options.UseSqlServer("Server=.;Database=CarsDB;Trusted_Connection=True;TrustServerCertificate=True;"));

//Sms Twilio 
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("TwilioSettings")
);
//«⁄œ«œ«  «·œ›⁄ 
builder.Services.Configure<PaymentSettings>(
    builder.Configuration.GetSection("PaymentSettings")
);
// Register repositories and services
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISmsService, TwilioSmsService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPaymentService, StripePaymentService>();

// Add controllers and Swagger services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy to allow any origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// =================================================================
// ≈÷«›… Œœ„«  JWT Authentication
// =================================================================
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ≈÷«›… Œœ„«  Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the development pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// =================================================================
// ≈÷«›… middleware «·„’«œﬁ… Ê«· Õﬂ„ ›Ì «·’·«ÕÌ« 
// ÌÃ» √‰ Ì√ Ì  — Ì»Â„ ﬁ»· MapControllers
// =================================================================
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();