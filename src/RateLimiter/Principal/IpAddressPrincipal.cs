using System.Net;

namespace RateLimiter.Principal
{
    public record IpAddressPrincipal(IPAddress IpAddress) : IPrincipal;
}
