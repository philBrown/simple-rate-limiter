using Microsoft.Extensions.Caching.Memory;
using RateLimiter.Principal;

namespace RateLimiter.Bucket
{
    public class InMemoryBucketCache : IBucketCache
    {
        private readonly Bandwidth _bandwidth;
        private readonly IMemoryCache _cache;

        public InMemoryBucketCache(Bandwidth bandwidth, IMemoryCache cache)
        {
            _bandwidth = bandwidth;
            _cache = cache;
        }

        public TokenBucket GetOrCreate(IPrincipal principal)
        {
            return _cache.GetOrCreate(principal, entry =>
            {
                entry.SlidingExpiration = _bandwidth.Duration;
                return new TokenBucket(_bandwidth);
            });
        }
    }
}
