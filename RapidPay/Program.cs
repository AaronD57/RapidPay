using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Data;
using RapidPay.Interfaces;
using RapidPay.Models;
using System.Text;
using RapidPay.services;

var builder = WebApplication.CreateBuilder(args);


var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if (string.IsNullOrEmpty(jwtSettings.SecretKey))
{
    throw new ArgumentNullException(nameof(jwtSettings.SecretKey), "SecretKey cannot be null or empty");
}

var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


builder.Services.AddSingleton<JwtTokenService>(); 
builder.Services.AddScoped<ICardService, CardServices>();
builder.Services.AddScoped<UserService>();


var connectionString = builder.Configuration.GetConnectionString("RapidPayDatabase");
Console.WriteLine($"Connection String: {connectionString}");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddControllers();



builder.Services.AddScoped<ICardService, CardServices>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUniversalFeeExchange>(provider => UniversalFeeExchange.Instance);
builder.Services.AddScoped<ICardService, CardServices>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization(); 
app.UseHttpsRedirection();


app.MapControllers();


app.Run();
