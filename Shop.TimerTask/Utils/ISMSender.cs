namespace Shop.TimerTask.Utils
{
    public interface ISMSender
    {
        bool SendMsgCode(string mobile, string code);
        bool SendMsgNewOrder(string mobile, string time);
        bool SendMsgResetPayPassword(string mobile);
    }
}
