using System;
using Microsoft.Extensions.Internal;

namespace RateLimiter.Bucket
{
    public class TokenBucket
    {
        private readonly Bandwidth _bandwidth;
        private readonly ISystemClock _clock;

        private int _availableTokens;
        private DateTimeOffset _lastRefill;

        public TokenBucket(Bandwidth bandwidth) : this(bandwidth, new SystemClock())
        {
        }

        public TokenBucket(Bandwidth bandwidth, ISystemClock clock)
        {
            _bandwidth = bandwidth;
            _clock = clock;

            _availableTokens = bandwidth.Capacity;
            _lastRefill = _clock.UtcNow;
        }

        public bool TryConsume(int tokens = 1)
        {
            if (tokens < 1 || tokens > _bandwidth.Capacity)
            {
                throw new ArgumentException($"Requested tokens must be between 1 and {_bandwidth.Capacity}",
                    nameof(tokens));
            }

            TryRefill();
            if (_availableTokens < tokens)
            {
                return false;
            }

            _availableTokens -= tokens;
            return true;
        }

        private void TryRefill()
        {
            var now = _clock.UtcNow;
            if (now > _lastRefill.Add(_bandwidth.Duration))
            {
                _availableTokens = _bandwidth.Capacity;
                _lastRefill = now;
            }
        }
    }
}
