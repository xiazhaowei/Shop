using Microsoft.Owin.Security.OAuth;
using Shop.QueryServices.Dapper;
using System.Security.Claims;
using System.Threading.Tasks;
using Xia.Common.Secutiry;

namespace Shop.Api
{
    /// <summary>
    /// Token 验证 
    /// 未使用，用户登录信息没有存储到token
    /// </summary>
    public class SimpleAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Factory.StartNew(() => context.Validated());
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
                await Task.Factory.StartNew(() => context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" }));
                
                // 对用户名、密码进行数据校验
                using (UserQueryService _userQueryServices = new UserQueryService())
                {
                    var userinfo = _userQueryServices.FindUser(context.UserName);
                    if (userinfo == null)
                    {
                        context.SetError("invalid_grant", "The user name is incorrect.");
                         return;
                    }
                    if (!PasswordHash.ValidatePassword(context.Password, userinfo.Password))
                    {
                        context.SetError("invalid_grant", "The user password is incorrect.");
                         return;
                    }
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", "user"));
                context.Validated(identity);
        }
    }
}