using System;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using RateLimiter.Bucket;
using RateLimiter.Principal;
using Xunit;

namespace RateLimiter.UnitTest
{
    public class InMemoryBucketCacheTest
    {
        private const long Address = 2130706433; // localhost

        private readonly IBucketCache _cache;

        public InMemoryBucketCacheTest()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cache = new InMemoryBucketCache(new Bandwidth(1, TimeSpan.FromSeconds(1)), memoryCache);
        }

        [Fact]
        public void Ensure_PrincipalValueObjects_ResultInTheSameCacheHit()
        {
            var principal1 = new IpAddressPrincipal(new IPAddress(Address));
            var principal2 = new IpAddressPrincipal(new IPAddress(Address));

            var bucket1 = _cache.GetOrCreate(principal1);
            var bucket2 = _cache.GetOrCreate(principal2);
            
            Assert.Same(bucket1, bucket2);
        }
    }
}
