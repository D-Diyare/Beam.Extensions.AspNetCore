namespace Beam.Extensions.AspNetCore;

public static class LocalizationRegistration
{

    /// <summary>
    /// Add localization service to the ASP.NEt core project.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="resourcePath">Resource folder name that contains .resex files, default: Resources.</param>
    /// <returns>IService collection after registering the localization.</returns>
    public static IServiceCollection AddLocalizationResource(this IServiceCollection services, string resourcePath = "Resources")
    {
        services.AddLocalization(options => options.ResourcesPath = resourcePath);
        return services;
    }

    /// <summary>
    /// Configure localization cultures.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="cultures">Cultures to add to the localization.</param>
    /// <param name="mainCulture">Defining the main culture for the application.</param>
    /// <returns>IService collection after configuring the localization.</returns>
    public static IServiceCollection ConfigureLocalization(this IServiceCollection services, IEnumerable<string> cultures,
        string mainCulture)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = cultures.Select(culture => new CultureInfo(culture)).ToList();

            options.DefaultRequestCulture = new RequestCulture(culture: mainCulture, uiCulture: mainCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        return services;
    }

    /// <summary>
    /// Using localization middleware.
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication</returns>
    public static IApplicationBuilder UseLocalization(this WebApplication app)
    {
        var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(locOptions.Value);
        return app;
    }
}
