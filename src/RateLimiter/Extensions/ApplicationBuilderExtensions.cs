using Microsoft.AspNetCore.Builder;

namespace RateLimiter.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRateLimiter(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
