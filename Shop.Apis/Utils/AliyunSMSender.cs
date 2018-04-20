using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using ECommon.Components;

namespace Shop.Apis.Utils
{
    /// <summary>
    /// 短信接口
    /// </summary>
    [Component]
    public  class AliyunSMSender:ISMSender
    {
        private static string product = "Dysmsapi";//短信API产品名称
        private static string domain = "dysmsapi.aliyuncs.com";//短信API产品域名
        private static string accessKeyId = "LTAItX7F19XYP9w5";//你的accessKeyId
        private static string accessKeySecret = "NuW1BiPbBRxGTYvyB6zASvjUccaDOd";//你的accessKeySecret
        
        private bool Send(string mobile,string template,string param)
        {
            var result = true;
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();

            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mobile;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "五福天下";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = template;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = param;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                //SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                result = false;
            }
            catch (ClientException e)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool SendMsgCode(string mobile,string code)
        {
            return Send(mobile, "SMS_86305021", "{\"code\":\""+code+"\"}");
        }

        public bool SendMsgNewOrder(string mobile, string name,string time)
        {
            return Send(mobile, "SMS_105630003", "{\"name\":\"" + name + "\",\"time\":\""+time+"\"}");
        }
    }
}