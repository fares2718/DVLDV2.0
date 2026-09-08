using DVLD.Application.Abstractions.Persistence;
using DVLD.Infrastructure.Persistence.Context;
using DVLD.Infrastructure.Persistence.Repositories;
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        return services;
    }
}