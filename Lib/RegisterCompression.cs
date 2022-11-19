using Beam.Extensions.AspNetCore.Helpers;

namespace Beam.Extensions.AspNetCore;

public static class RegisterCompression
{
    /// <summary>
    /// Registers response compression using brotlic & gzip providers, you can find the list of extensions to be compressed in the repo (MimeTypes).
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>IService collection after registering the response compression.</returns>
    public static IServiceCollection RegisterResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = MimeTypes.CommonMimeTypes;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        return services;
    }

    /// <summary>
    /// Registers response compression using brotlic & gzip providers, you can find the list of extensions to be compressed in the repo (MimeTypes).
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="options">Options to configure the compression registration</param>
    /// <returns>IService collection after register the compression.</returns>
    public static IServiceCollection RegisterResponseCompression(this IServiceCollection services, Action<ResponseCompressionOptions> options)
    {
        services.AddResponseCompression(options);
        return services;
    }

    /// <summary>
    /// Configures response compression, default: brotlic compression = Fastest, gizp compression = SmallestSize
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>IService collection after the compression is configured.</returns>
    public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
    {
        services.Configure<BrotliCompressionProviderOptions>
            (options => options.Level = CompressionLevel.Fastest);

        services.Configure<GzipCompressionProviderOptions>
            (options => options.Level = CompressionLevel.SmallestSize);

        return services;
    }

    /// <summary>
    /// Configures response compression based on given levels.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="brotlicCompressionLevel">Brotlic Compression Level</param>
    /// <param name="gzipCompressionLevel">Gzip Compression Level</param>
    /// <typeparam name="TBrotlic">Brotlic Compression Level</typeparam>
    /// <typeparam name="TGzip">Gzip Compression Level</typeparam>
    /// <returns>IService collection after the compression is configured.</returns>
    public static IServiceCollection ConfigureResponseCompression<TBrotlic, TGzip>(this IServiceCollection services,
        CompressionLevel brotlicCompressionLevel, CompressionLevel gzipCompressionLevel)
        where TBrotlic : BrotliCompressionProviderOptions
        where TGzip : GzipCompressionProviderOptions
    {
        services.Configure<TBrotlic>(options => options.Level = brotlicCompressionLevel);
        services.Configure<TGzip>(options => options.Level = gzipCompressionLevel);

        return services;
    }
}
