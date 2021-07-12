namespace RateLimiter.Bucket
{
    public interface ITokenBucket
    {
        bool TryConsume(int tokens = 1);
        int SecondsToNextRefill { get; }
    }
}
