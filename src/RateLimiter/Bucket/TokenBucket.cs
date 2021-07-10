using System;

namespace RateLimiter.Bucket
{
    public class TokenBucket
    {
        private readonly Bandwidth _bandwidth;
        
        private int _availableTokens;
        private DateTime _lastRefill;

        public TokenBucket(Bandwidth bandwidth)
        {
            _bandwidth = bandwidth;

            _availableTokens = bandwidth.Capacity;
            _lastRefill = DateTime.Now;
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
            var now = DateTime.Now;
            if (now > _lastRefill.Add(_bandwidth.Duration))
            {
                _availableTokens = _bandwidth.Capacity;
                _lastRefill = now;
            }
        }
    }
}
