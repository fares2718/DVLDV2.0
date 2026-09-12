using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.Abstractions.Services;
using DVLD.Infrastructure.Persistence.Context;
using DVLD.Infrastructure.Persistence.Repositories;
using DVLD.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        
        //Services
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}