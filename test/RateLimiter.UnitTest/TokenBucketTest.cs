using System;
using Microsoft.Extensions.Internal;
using Moq;
using RateLimiter.Bucket;
using Xunit;

namespace RateLimiter.UnitTest
{
    public class TokenBucketTest
    {
        private readonly Mock<ISystemClock> _clock = new();

        [Fact]
        public void TokenBucket_AllowsConsumption_WhenCapacityIsAvailable_AndPreventsConsumption_WhenCapacityExhausted()
        {
            _clock.Setup(c => c.UtcNow).Returns(DateTimeOffset.UtcNow);
            var bandwidth = new Bandwidth(1, TimeSpan.FromSeconds(1));
            var bucket = new TokenBucket(bandwidth, _clock.Object);

            Assert.True(bucket.TryConsume());
            Assert.False(bucket.TryConsume());
        }

        [Fact]
        public void TokenBucket_AllowsConsumption_AfterRefill()
        {
            var now = DateTimeOffset.UtcNow;
            _clock.SetupSequence(c => c.UtcNow)
                .Returns(now)
                .Returns(now)
                .Returns(now.AddSeconds(1))
                .Returns(now.AddSeconds(2));

            var bandwidth = new Bandwidth(1, TimeSpan.FromSeconds(1));
            var bucket = new TokenBucket(bandwidth, _clock.Object);

            Assert.True(bucket.TryConsume());
            Assert.False(bucket.TryConsume());
            Assert.True(bucket.TryConsume());
        }

        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0.5, 1)]
        [InlineData(2, 1000, 1)]
        public void TimeToNextRefill_ReturnsTimeInSeconds(int durationSeconds, double waitMillis,
            int expectedTimeToRefill)
        {
            var now = DateTimeOffset.UtcNow;

            _clock.SetupSequence(c => c.UtcNow)
                .Returns(now)
                .Returns(now.AddMilliseconds(waitMillis));
            
            var bandwidth = new Bandwidth(1, TimeSpan.FromSeconds(durationSeconds));
            var bucket = new TokenBucket(bandwidth, _clock.Object);
            
            Assert.Equal(expectedTimeToRefill, bucket.SecondsToNextRefill);
        }
    }
}
