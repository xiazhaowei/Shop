using Microsoft.AspNetCore.Http;
using System;

namespace Shop.Apis.Services
{
    public interface IContextService
    {
        AccountIdentity GetCurrentAccount(HttpContext httpContext);
    }

    public class AccountIdentity
    {
        public string UserId { get; private set; }
        public string WalletId { get; private set; }
        public string Mobile { get; private set; }

        public AccountIdentity(string userId,string walletId, string mobile)
        {
            UserId = userId;
            WalletId = walletId;
            Mobile = mobile;
        }
    }
}
