using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Shop.WeiXin.Services.impl
{
    /// <summary>
    /// 微信二维码服务
    /// </summary>
    public class QrCodeService:MpServiceBase,IQrCodeService
    {
        /// <summary>
        /// 创建分享二维码
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string CreateShareQrCode(string openId)
        {
            var accessToken = AccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }
            var qrResult = QrCodeApi.Create(accessToken, 10000, 111, QrCode_ActionName.QR_LIMIT_STR_SCENE, openId);
            var qrCodeUrl = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            return qrCodeUrl;
        }
    }
}