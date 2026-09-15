using System.Text;
using DVLD.Application.Abstractions.Authentication;
using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.Abstractions.Services;
using DVLD.Infrastructure.Authentication;
using DVLD.Infrastructure.Persistence.Context;
using DVLD.Infrastructure.Persistence.Repositories;
using DVLD.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DVLD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DvldContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DeveloperConnection")));
        //Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        //Repositories
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
        
        //Services
        services.AddScoped<IImageService, ImageService>();
        
        //JWT
        services.AddScoped<ITokensGenerator, TokensGenerator>();
        services.AddAuthentication(confOp =>
       {
           confOp.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           confOp.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           confOp.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       //    .AddCookie(confOp =>
       //    {
       //        confOp.Cookie.Name = "token";
       //        confOp.Events.OnRedirectToLogin = context =>
       //        {
       //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
       //            return Task.CompletedTask;
       //        };
       //    })
       .AddJwtBearer(confOp =>
       {
           confOp.RequireHttpsMetadata = false;
           confOp.SaveToken = true;
           confOp.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["JwtSettings:Issuer"],
               ValidAudience = configuration["JwtSettings:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!))
           };
           confOp.Events = new JwtBearerEvents
           {

               OnMessageReceived = context =>
               {
                   var token = context.Request.Cookies["token"];
                   if (!string.IsNullOrEmpty(token))
                   {
                       context.Token = token;
                   }
                   return Task.CompletedTask;
               },
               OnChallenge = context =>
        {
            context.HandleResponse(); // Skip the default redirect logic
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return Task.CompletedTask;
        }
           };
       });

        return services;
    }
}