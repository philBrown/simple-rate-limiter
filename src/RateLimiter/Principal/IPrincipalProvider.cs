using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RateLimiter.Principal
{
    public interface IPrincipalProvider
    {
        Task<IPrincipal> GetPrincipal(HttpContext context);
    }
}
