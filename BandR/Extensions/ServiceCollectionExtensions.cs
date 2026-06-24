using BandR.Data;
using BandR.Services;
using BandR.Services.Interfaces;
using BandR.Validators.Musicians;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BandR.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMusicianService, MusicianService>();
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