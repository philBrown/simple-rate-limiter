using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using RateLimiter.Bucket;
using RateLimiter.Principal;
using Xunit;

namespace RateLimiter.UnitTest
{
    public class RateLimiterMiddlewareTest
    {
        private readonly Mock<IPrincipalProvider> _principalProvider = new();
        private readonly Mock<IBucketCache> _cache = new();
        private readonly Mock<ITokenBucket> _bucket = new();
        private readonly Mock<RequestDelegate> _next = new();
        private readonly Mock<HttpContext> _context = new ();
        private readonly Mock<HttpResponse> _response = new();
        private readonly RateLimiterMiddleware _middleware;

        public RateLimiterMiddlewareTest()
        {
            _middleware = new RateLimiterMiddleware(_cache.Object, _next.Object, _principalProvider.Object);
            
            _context.Setup(c => c.Response)
                .Returns(_response.Object);
            _cache.Setup(c => c.GetOrCreate(It.IsAny<IPrincipal>()))
                .Returns(_bucket.Object);
            _response.Setup(r => r.Body).Returns(Mock.Of<Stream>());
        }

        [Fact]
        public async Task RequestsAreAllowed_WhenBucketHasCapacity()
        {
            var principal = SetupMocks("test", true);

            await _middleware.Invoke(_context.Object);
            
            _principalProvider.Verify(p => p.GetPrincipal(_context.Object));
            _cache.Verify(c => c.GetOrCreate(principal));
            _next.Verify(d => d(_context.Object));
        }

        [Fact]
        public async Task RequestsAreDenied_WhenBucketIsEmpty()
        {
            SetupMocks("test", false);

            await _middleware.Invoke(_context.Object);
            
            _next.Verify(d => d(It.IsAny<HttpContext>()), Times.Never);
            _response.VerifySet(r => r.StatusCode = 429);
        }

        private IPrincipal SetupMocks(string principalId, bool hasCapacity)
        {
            var principal = new TestPrincipal(principalId);
            _bucket.Setup(b => b.TryConsume(It.IsAny<int>()))
                .Returns(hasCapacity);
            _principalProvider.Setup(p => p.GetPrincipal(It.IsAny<HttpContext>()))
                .ReturnsAsync(principal);
            return principal;
        }
    }

    internal record TestPrincipal(string Id) : IPrincipal;
}
