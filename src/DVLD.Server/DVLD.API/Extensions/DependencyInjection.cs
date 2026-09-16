namespace DVLD.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DVLD.Server.CORS", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        services.AddControllers();

        services.AddProblemDetails();

        services.AddHttpContextAccessor();

        services.AddAuthentication();

        return services;
    }
}