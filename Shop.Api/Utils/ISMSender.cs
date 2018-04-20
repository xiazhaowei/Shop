using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Utils
{
    public interface ISMSender
    {
        bool SendMsgCode(string mobile, string code);
        bool SendMsgNewOrder(string mobile, string name, string time);
    }
}