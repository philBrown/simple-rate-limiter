using System;
using Microsoft.Extensions.Caching.Memory;
using RateLimiter.Principal;

namespace RateLimiter.Bucket
{
    /// <summary>
    /// Maintains a thread-safe, in-memory cache of buckets
    /// </summary>
    public class InMemoryBucketCache : IBucketCache, IDisposable
    {
        private readonly Bandwidth _bandwidth;
        private readonly IMemoryCache _cache;

        public InMemoryBucketCache(Bandwidth bandwidth, IMemoryCache cache)
        {
            _bandwidth = bandwidth;
            _cache = cache;
        }

        public ITokenBucket GetOrCreate(IPrincipal principal)
        {
            return _cache.GetOrCreate(principal, entry =>
            {
                entry.SlidingExpiration = _bandwidth.Duration;
                return new TokenBucket(_bandwidth);
            });
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
