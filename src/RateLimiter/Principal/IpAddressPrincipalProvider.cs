using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RateLimiter.Principal
{
    /// <summary>
    /// An implementation of the principal provider that extracts the source IP address from HTTP requests
    /// </summary>
    public class IpAddressPrincipalProvider : IPrincipalProvider
    {
        private readonly ILogger<IpAddressPrincipalProvider> _logger;

        public IpAddressPrincipalProvider(ILogger<IpAddressPrincipalProvider> logger)
        {
            _logger = logger;
        }

        public Task<IPrincipal> GetPrincipal(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            _logger.LogDebug("Resolved IP Principal {IpAddress}", ipAddress);
            return Task.FromResult(new IpAddressPrincipal(ipAddress) as IPrincipal);
        }
    }
}
