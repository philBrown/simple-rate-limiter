using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RateLimiter.Principal
{
    public class IpAddressPrincipalProvider : IPrincipalProvider
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<IpAddressPrincipalProvider> _logger;

        public IpAddressPrincipalProvider(IHttpContextAccessor context, ILogger<IpAddressPrincipalProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IPrincipal GetPrincipal()
        {
            var ipAddress = _context.HttpContext.Connection.RemoteIpAddress;
            _logger.LogDebug("Resolved IP Principal {IpAddress}", ipAddress);
            return new IpAddressPrincipal(ipAddress);
        }
    }
}
