using System.Security.Claims;
using ECommon.Components;
using Microsoft.AspNetCore.Http;

namespace Shop.Apis.Services
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
