namespace Shop.Apis.Utils
{
    public  interface ISMSender
    {
        bool SendMsgCode(string mobile, string code);
        bool SendMsgNewOrder(string mobile, string name, string time);
    }
}
