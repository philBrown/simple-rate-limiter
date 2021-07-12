using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RateLimiter.Principal
{
    /// <summary>
    /// Principal provider service
    /// </summary>
    public interface IPrincipalProvider
    {
        /// <summary>
        /// Given an HTTP context (request), identify the principal entity
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Request principal entity</returns>
        Task<IPrincipal> GetPrincipal(HttpContext context);
    }
}
