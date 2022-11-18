namespace Beam.Extensions.AspNetCore;

public static class RegisterContext
{
    /// <summary>
    /// Register EF Core DbContext (scoped) with sql server and the given connection string.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="connectionStringName">Connection String name from appSettings.json</param>
    /// <param name="timeOut">connection time out.</param>
    /// <typeparam name="TContext">DbContext to register.</typeparam>
    /// <returns>IService collection after registering the DbContext.</returns>
    public static IServiceCollection RegisterDbContext<TContext>(this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName,
        int timeOut = 30)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            options.UseSqlServer(connectionString, sqlOption =>
            {
                sqlOption.CommandTimeout(timeOut);
            });

        });
        return services;
    }
}