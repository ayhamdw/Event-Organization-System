using System.Text;
using dotenv.net;
using Event_Organization_System.IServices;
using Event_Organization_System.Services;
using EventOrganizationSystem.Generic;
using EventOrganizationSystem.Helper;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.Middleware;
using EventOrganizationSystem.model;
using EventOrganizationSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();
var AllowedOrigins = "AllowedWebsiteOrigin";

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new InvalidOperationException("Missing connection string");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins, policy =>
    {
        policy.WithOrigins("https://localhost:5173", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = ValidationErrorExtractor.GetValidationErrors(context);
            var response = GeneralApiResponse<string>.Failure(errors);
            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IJwtService, JwtServices>();
builder.Services.AddScoped<IValidatePasswordServices, ValidatePasswordServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IEventServices, EventServices>();
builder.Services.AddScoped<ITicketServices, TicketServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(AllowedOrigins);
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event OrganizationSystem API V1");
    c.RoutePrefix = string.Empty;
    c.InjectStylesheet("/swagger-ui/custom.css");
    c.InjectJavascript("/swagger-ui/custom.js");
});


app.Run();
