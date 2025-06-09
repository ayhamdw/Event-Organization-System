using System.Text;
using dotenv.net;
using Event_Organization_System.Generic;
using Event_Organization_System.Helper;
using Event_Organization_System.IServices;
using Event_Organization_System.Middleware;
using Event_Organization_System.model;
using Event_Organization_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (connectionString == null)
{
    throw new InvalidOperationException("Missing connection string");
}

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

builder.Services.AddScoped<IAuthServices , AuthServices>();
builder.Services.AddScoped<IJwtService, JwtServices>();
builder.Services.AddScoped<IValidatePasswordServices, ValidatePasswordServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IEventServices, EventServices>();
builder.Services.AddScoped<ITicketServices, TicketServices>();

var app = builder.Build();
app.UseStaticFiles();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Organization System API V1");
        c.RoutePrefix = string.Empty;

        c.InjectStylesheet("/swagger-ui/custom.css");
        c.InjectJavascript("/swagger-ui/custom.js");

    });
}




app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();


app.Run();
