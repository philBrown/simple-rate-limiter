using RateLimiter.Principal;

namespace RateLimiter.Bucket
{
    /// <summary>
    /// Cache service mapping principals to token buckets
    /// </summary>
    public interface IBucketCache
    {
        ITokenBucket GetOrCreate(IPrincipal principal);
    }
}
