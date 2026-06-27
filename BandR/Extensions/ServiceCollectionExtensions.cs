using System.Text;
using BandR.Configuration;
using BandR.Data;
using BandR.Entities;
using BandR.Services;
using BandR.Services.Interfaces;
using BandR.Validators.Musicians;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BandR.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        var jwtConfig = config.GetSection("Jwt").Get<JwtConfiguration>()
                        ?? throw new InvalidOperationException("JWT configuration is missing");
        services.Configure<JwtConfiguration>(config.GetSection("Jwt"));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        var problem = new ProblemDetails
                        {
                            Type = "Auth/Unauthenticated",
                            Status = StatusCodes.Status401Unauthorized
                        };
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsJsonAsync(problem);
                    },
                    OnForbidden = async context =>
                    {
                        var problem = new ProblemDetails
                        {
                            Type = "Auth/Forbidden",
                            Status = StatusCodes.Status403Forbidden
                        };
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsJsonAsync(problem);
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = jwtConfig!.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                };
            });

        services.AddAuthorization();
    
        
        services.AddScoped<IMusicianService, MusicianService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITokenService, TokenService>();
        // services.AddScoped<IAnnouncementService, AnnouncementService>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("Default"));
        });
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateMusicianDtoValidator>();
        return services;
    }
}