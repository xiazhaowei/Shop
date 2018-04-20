using ECommon.Components;
using System.Security.Claims;
using System.Web;

namespace Shop.AdminApi.Services
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
