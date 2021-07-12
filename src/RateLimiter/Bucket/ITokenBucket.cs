namespace RateLimiter.Bucket
{
    /// <summary>
    /// The token bucket interface
    /// </summary>
    public interface ITokenBucket
    {
        /// <summary>
        /// Attempt to consume the provided number of tokens
        /// </summary>
        /// <param name="tokens">Consume this many tokens, defaults to 1</param>
        /// <returns>True if token capacity has not been exhausted, false otherwise</returns>
        bool TryConsume(int tokens = 1);
        
        /// <summary>
        /// Indicates the time remaining in seconds until this bucket is refilled
        /// </summary>
        int SecondsToNextRefill { get; }
    }
}
