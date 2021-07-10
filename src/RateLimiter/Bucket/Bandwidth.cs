using System;

namespace RateLimiter.Bucket
{
    public record Bandwidth(int Capacity, TimeSpan Duration);
}
