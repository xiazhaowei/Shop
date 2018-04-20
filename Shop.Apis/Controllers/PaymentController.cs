using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request.Payment;
using Shop.Api.Models.Response;
using Shop.Apis.AliPayAPI;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Commands.Payments;
using Shop.QueryServices;
using Shop.QueryServices.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xia.Common.Extensions;


namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
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
        [Route("PaymentAccepted")]
        public async Task<BaseApiResponse> PaymentAccepted([FromBody]ProcessorPaymentRequest request)
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
        [Route("PaymentRejected")]
        public async Task<BaseApiResponse> PaymentRejected([FromBody]ProcessorPaymentRequest request)
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
        [Route("AliPay")]
        public BaseApiResponse AliPay([FromBody]PaymentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var orderInfo = AliPayApi.GetAlipayOrderInfo(request.Amount,DateTime.Now.ToSerialNumber());

            return new BaseApiResponse {
                Message= orderInfo
            };
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