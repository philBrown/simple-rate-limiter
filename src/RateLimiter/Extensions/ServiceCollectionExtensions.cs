using System.ComponentModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RateLimiter.Bucket;
using RateLimiter.Principal;

namespace RateLimiter.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, RateLimitOptions options)
        {
            var bandwidth = new Bandwidth(options.Capacity, options.Duration);

            services.AddSingleton<IPrincipalProvider>(sp =>
            {
                return options.Principal switch
                {
                    RateLimitOptions.PrincipalType.IpAddress => new IpAddressPrincipalProvider(
                        sp.GetRequiredService<ILogger<IpAddressPrincipalProvider>>()),
                    _ => throw new InvalidEnumArgumentException(nameof(options.Principal))
                };
            });

            services.AddSingleton<IBucketCache>(sp =>
                new InMemoryBucketCache(bandwidth, new MemoryCache(new MemoryCacheOptions())));

            return services;
        }
    }
}
