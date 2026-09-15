namespace DVLD.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddProblemDetails();

        services.AddHttpContextAccessor();

        services.AddAuthentication();

        return services;
    }
}