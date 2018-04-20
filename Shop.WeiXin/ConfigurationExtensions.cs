using ECommon.Configurations;
using Shop.WeiXin.Services;
using Shop.WeiXin.Services.impl;

namespace Shop.WeiXin
{
    public static class ConfigurationExtensions
    {
        /// <summary>.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseMyComponents(this Configuration configuration)
        {
            configuration.SetDefault<IUserQueryService, UserQueryService>(new UserQueryService());
            configuration.SetDefault<IUserGenerator, UserGenerator>(new UserGenerator());
            configuration.SetDefault<ICreateMenuService, CreateMenuService>(new CreateMenuService());
            configuration.SetDefault<ICreateMenuService, CreateMenuService>(new CreateMenuService());
            configuration.SetDefault<IQrCodeService, QrCodeService>(new QrCodeService());
            configuration.SetDefault<ITemplateMessageService, TemplateMessageService>(new TemplateMessageService());
            configuration.SetDefault<IUserOAuthService, UserOAuthService>(new UserOAuthService());
            configuration.SetDefault<ICustomService, CustomService>(new CustomService());

            return configuration;
        }
        
    }
}
