using System;

namespace RateLimiter.Bucket
{
    /// <summary>
    /// Encapsulates token capacity and refill frequency
    /// </summary>
    /// <param name="Capacity"></param>
    /// <param name="Duration"></param>
    public record Bandwidth(int Capacity, TimeSpan Duration);
}
