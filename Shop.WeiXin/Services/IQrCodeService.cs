namespace Shop.WeiXin.Services
{
    public interface IQrCodeService
    {
        string CreateShareQrCode(string openId);
    }
}
