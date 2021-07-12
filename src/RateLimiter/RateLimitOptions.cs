using System;

namespace RateLimiter
{
    public record RateLimitOptions
    {
        public enum PrincipalType
        {
            IpAddress
        }

        public PrincipalType Principal { get; init; } = PrincipalType.IpAddress;

        public int Capacity { get; init; } = 100;

        public TimeSpan Duration { get; init; } = TimeSpan.FromHours(1);
    }
}
