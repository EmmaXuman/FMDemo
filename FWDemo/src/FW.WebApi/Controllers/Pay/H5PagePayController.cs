using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.Models.RequestModel;
using FW.Services.Pay;
using FW.WebCore.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FW.WebApi.Controllers.Pay
{
    [Route("api/Pay/[controller]")]
    [ApiController]
    public class H5PagePayController : ControllerBase
    {
        private readonly PayServiceContext _payServiceContext;
        private const string SERVICETYPE = "H5PagePay";

        public H5PagePayController( PayServiceContext payServiceContext )
        {
            _payServiceContext = payServiceContext;
        }

        [HttpPost]
        public ExecuteResult<PayRes> WechatPrePareToPayForH5( H5PageAliPayReq req )
        {
            var payReq = new PayReq
            {
                LocalOrderId = req.OrderId,
                TerminalIp = GetUserIp(),
                AsyncNotifyUrl = _payServiceContext.BuildPubLessonPaymentNotifyUrl(PayChanel.WeChatPay),
                SyncCallbackUrl = req.ReturnUrl,
                PayAmount = req.PayAmount,
                Type = req.Type
            };
            var result = _payServiceContext.PrePareTpPay(SERVICETYPE, PayMethod.H5, PayChanel.WeChatPay, payReq);
            return new ExecuteResult<PayRes>().Set(true, "请求支付成功", result);
        }

        [HttpPost]
        public ExecuteResult<PayRes> AliPrePareToPayForH5( H5PageAliPayReq req )
        {
            var payReq = new PayReq
            {
                LocalOrderId = req.OrderId,
                AsyncNotifyUrl = _payServiceContext.BuildPubLessonPaymentNotifyUrl(PayChanel.AliPay),
                SyncCallbackUrl = req.ReturnUrl,
                PayAmount = req.PayAmount,
                Type = req.Type
            };
            var result = _payServiceContext.PrePareTpPay(SERVICETYPE, PayMethod.H5, PayChanel.AliPay, payReq);
            return new ExecuteResult<PayRes>().Set(true, "请求支付成功", result);
        }


        /// <summary>
        /// 主动回调验证订单支付成功
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        [HttpGet]
        public ExecuteResult OrderCallback( string tradeNo )
        {
            var serviceResult = _payServiceContext.OrderCallback(SERVICETYPE, tradeNo);
            return serviceResult;
        }

        private string GetUserIp()
        {
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];
            return remoteIpAddress;
        }
    }
}
