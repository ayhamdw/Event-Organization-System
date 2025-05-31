using System.Text;
using dotenv.net;
using Event_Organization_System.controller;
using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (connectionString == null)
{
    throw new InvalidOperationException("Missing connection string.");
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString , ServerVersion.AutoDetect(connectionString))
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddScoped<ILoginService , LoginServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();


app.Run();
