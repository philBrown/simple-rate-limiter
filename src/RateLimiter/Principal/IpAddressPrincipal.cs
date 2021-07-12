using System.Net;

namespace RateLimiter.Principal
{
    /// <summary>
    /// Identifies request entities by source IP address
    /// </summary>
    /// <param name="IpAddress">Request source IP address</param>
    public record IpAddressPrincipal(IPAddress IpAddress) : IPrincipal;
}
