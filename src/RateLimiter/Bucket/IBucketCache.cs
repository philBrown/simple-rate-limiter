using RateLimiter.Principal;

namespace RateLimiter.Bucket
{
    public interface IBucketCache
    {
        ITokenBucket GetOrCreate(IPrincipal principal);
    }
}
