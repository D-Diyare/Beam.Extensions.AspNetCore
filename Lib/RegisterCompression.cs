using Beam.Extensions.AspNetCore.Helpers;

namespace Beam.Extensions.AspNetCore;

public static class RegisterCompression
{
    /// <summary>
    /// Registers response compression using Brotli & GZip providers, you can find the list of extensions to be compressed in the repo (MimeTypes).
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
    /// Registers response compression using Brotli & GZip providers, you can find the list of extensions to be compressed in the repo (MimeTypes).
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
    /// Configures response compression, default: Brotli compression = Fastest, GZip compression = SmallestSize
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
    /// <param name="brotliCompressionLevel">Brotli Compression Level</param>
    /// <param name="gzipCompressionLevel">Gzip Compression Level</param>
    /// <typeparam name="TBrotli">Brotli Compression Level</typeparam>
    /// <typeparam name="TGzip">Gzip Compression Level</typeparam>
    /// <returns>IService collection after the compression is configured.</returns>
    public static IServiceCollection ConfigureResponseCompression<TBrotli, TGzip>(this IServiceCollection services,
        CompressionLevel brotliCompressionLevel, CompressionLevel gzipCompressionLevel)
        where TBrotli : BrotliCompressionProviderOptions
        where TGzip : GzipCompressionProviderOptions
    {
        services.Configure<TBrotli>(options => options.Level = brotliCompressionLevel);
        services.Configure<TGzip>(options => options.Level = gzipCompressionLevel);

        return services;
    }
}
