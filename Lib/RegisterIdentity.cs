namespace Beam.Extensions.AspNetCore;

public static class RegisterIdentity
{
    /// <summary>
    /// Registers identity service based on given options.
    /// </summary>
    /// <param name="services">IService collection</param>
    /// <param name="config">Identity options</param>
    /// <typeparam name="TContext">DbContext to register.</typeparam>
    /// <typeparam name="TUser">Identity User (can be generic)</typeparam>
    /// <typeparam name="TRole">Identity User (can be role)</typeparam>
    /// <typeparam name="TType">Generic type of identity user and role.</typeparam>
    /// <returns>IService collection after adding the identity.</returns>
    public static IServiceCollection AddIdentityService<TContext, TUser, TRole, TType>(this IServiceCollection services,
        Action<IdentityOptions>? config = null)
        where TContext : DbContext
        where TType : IEquatable<TType>
        where TUser : IdentityUser<TType>
        where TRole : IdentityRole<TType>
    {

        services.AddIdentity<TUser, TRole>(config ?? ConfigureIdentity())
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });

        return services;
    }

    /// <summary>
    /// Configures token life span.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="lifeSpan">Time span to configure the token expiration.</param>
    /// <returns>IService collection after configuring of the TokenLifeSpan.</returns>
    public static IServiceCollection ConfigureDataProtection(this IServiceCollection services, TimeSpan lifeSpan)
    {
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = lifeSpan;
        });

        return services;
    }

    /// <summary>
    /// Configure Identity Options.
    /// </summary>
    /// <param name="config">Options to configure the identity.</param>
    /// <returns>Identity options</returns>
    private static Action<IdentityOptions> ConfigureIdentity(Action<IdentityOptions>? config = null)
    {
        if (config is not null)
            return config;

        return options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 5;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        };
    }

    /// <summary>
    /// Add JWT identity.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>IService collection after registering the JWT identity.</returns>
    public static IServiceCollection AddJwtService(this IServiceCollection services)
    {
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        });
        return services;
    }

    /// <summary>
    /// Add JWT identity.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configureOptions">Configure JWT bearer options.</param>
    /// <returns>IService collection after registering the JWT identity.</returns>
    public static IServiceCollection AddJwtService(this IServiceCollection services, Action<JwtBearerOptions> configureOptions)
    {
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions);
        return services;
    }
    
    
    /// <summary>
    /// Add policy authorizations based on given names and roles.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="policies">Policies to configure.</param>
    /// <returns>IService collection after adding the policies.</returns>
    public static IServiceCollection AddAuthorizations(this IServiceCollection services, Dictionary<string, string[]> policies)
    {
        foreach (var (name, roles) in policies)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(name, policy => policy.RequireRole(roles)
                    .RequireAuthenticatedUser());
            });
        }
        return services;
    }
}
