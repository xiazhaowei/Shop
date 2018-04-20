using ECommon.Components;
using System.Security.Claims;
using System.Web;

namespace Shop.Api.Services
{
    [Component]
    public class ContextService : IContextService
    {
        public AccountIdentity GetCurrentAccount(HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var walletId = identity.FindFirst(ClaimTypes.UserData).Value;
                var mobile = identity.FindFirst(ClaimTypes.MobilePhone).Value;
                return new AccountIdentity(userId,walletId, mobile);
            }
            return null;
        }
    }
}
