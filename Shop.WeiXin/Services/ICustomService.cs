namespace Shop.WeiXin.Services
{
    public interface ICustomService
    {
        void SendText(string openId, string message);
    }
}
