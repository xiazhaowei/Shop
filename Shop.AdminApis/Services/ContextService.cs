using System.Security.Claims;
using ECommon.Components;
using Microsoft.AspNetCore.Http;

namespace Shop.AdminApis.Services
{
    [Component]
    public class ContextService : IContextService
    {
        public AdminAccountIdentity GetCurrentAdmin(HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                var adminId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var name = identity.FindFirst(ClaimTypes.Name).Value;
                return new AdminAccountIdentity(adminId, name);
            }
            return null;
        }
    }
}
