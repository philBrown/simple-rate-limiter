using RateLimiter.Principal;

namespace RateLimiter.Bucket
{
    public interface IBucketCache
    {
        TokenBucket GetOrCreate(IPrincipal principal);
    }
}
