using ENode.Commanding;
using Shop.Api.AliPayAPI;
using Shop.Api.Extensions;
using Shop.Api.Models.Request.Payment;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Payments;
using Shop.Api.Services;
using Shop.Commands.Payments;
using Shop.QueryServices;
using Shop.QueryServices.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using Xia.Common.Extensions;


namespace Shop.Api.Controllers
{
    public class PaymentController: BaseApiController
    {
        private const int WaitTimeoutInSeconds = 5;

        private IPaymentQueryService _paymentQueryService;//Q 端

        public PaymentController(ICommandService commandService, IContextService contextService,
            IPaymentQueryService paymentQueryService) : base(commandService,contextService)
        {
            _paymentQueryService = paymentQueryService;
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> PaymentAccepted(ProcessorPaymentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var payment = _paymentQueryService.FindPayment(request.PaymentId);
            if (payment == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有支付项目" };
            }

            var command =new CompletePaymentCommand(
                new Common.PayInfo(request.Total,request.ShopCash)) {
                AggregateRootId = request.PaymentId
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 支付拒绝
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> PaymentRejected(ProcessorPaymentRequest request)
        {
            request.CheckNotNull(nameof(request));
            var payment = _paymentQueryService.FindPayment(request.PaymentId);
            if (payment == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有支付项目" };
            }

            var command=new CancelPaymentCommand { AggregateRootId = request.PaymentId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        
        /// <summary>
        /// 支付宝 支付
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse AliPay(PaymentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var orderInfo = AliPayApi.GetAlipayOrderInfo(request.Amount,DateTime.Now.ToSerialNumber());

            return new BaseApiResponse {
                Message= orderInfo
            };
        }
        
        /// <summary>
        /// 微信支付-公众号支付
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse WxPay(WxPaymentRequest request)
        {
            request.CheckNotNull(nameof(request));
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = request.OpenId;
            jsApiPay.total_fee = request.Amount;
            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();//统一下单
                var wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   这个要返回给客户端               
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                //在页面上显示订单信息
                //unifiedOrderResult.ToPrintStr();
                return new BaseApiResponse { Message = wxJsApiParam };
            }
            catch (Exception ex)
            {
                return new BaseApiResponse { Code = 400, Message = ex.Message };
            }
        }


        public BaseApiResponse GetOpenIdAndAccessToken(string code)
        {
            code.CheckNotNullOrEmpty(nameof(code));
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.GetOpenidAndAccessTokenFromCode(code);
            return new GetOpenIdAndAccessTokenResponse { Message = "", openid = jsApiPay.openid, access_token = jsApiPay.access_token };
        }
        #region 私有方法

        /// <summary>
        /// 直到获取付款信息
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        private Payment WaitUntilAvailable(Guid paymentId)
        {
            var deadline = DateTime.Now.AddSeconds(WaitTimeoutInSeconds);
            while (DateTime.Now < deadline)
            {
                var paymentDTO = _paymentQueryService.FindPayment(paymentId);
                if (paymentDTO != null)
                {
                    return paymentDTO;
                }
                Thread.Sleep(500);
            }
            return null;
        }

       
        #endregion
    }
}