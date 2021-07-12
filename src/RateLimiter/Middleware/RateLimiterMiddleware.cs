using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RateLimiter.Bucket;
using RateLimiter.Principal;

namespace RateLimiter.Middleware
{
    public class RateLimiterMiddleware
    {
        private readonly IBucketCache _bucketCache;
        private readonly RequestDelegate _next;
        private readonly IPrincipalProvider _principalProvider;

        public RateLimiterMiddleware(IBucketCache bucketCache, RequestDelegate next,
            IPrincipalProvider principalProvider)
        {
            _bucketCache = bucketCache;
            _next = next;
            _principalProvider = principalProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var principal = await _principalProvider.GetPrincipal(httpContext);
            var bucket = _bucketCache.GetOrCreate(principal);

            if (bucket.TryConsume())
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
                await httpContext.Response.WriteAsync(
                    $"Rate limit exceeded. Try again in #{bucket.SecondsToNextRefill} seconds");
            }
        }
    }
}
