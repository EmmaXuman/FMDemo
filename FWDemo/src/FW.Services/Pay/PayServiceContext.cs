using FW.Component.Pay;
using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.WebCore.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FW.Services.Pay
{
    public class PayServiceContext
    {
        private readonly IEnumerable<BasePayService> _payServices;
        private readonly PayComponentFactory _payComponentFactory;

        public PayServiceContext( IEnumerable<BasePayService> payServices,
            PayComponentFactory payComponentFactory )
        {
            _payServices = payServices;
            _payComponentFactory = payComponentFactory;
        }


        private BaseComponent BindEvent(string serviceType, PayChanel payChanel)
        {
            var payservice = _payServices.FirstOrDefault(s=>s.PayServiceType==serviceType);
            var basePayComponent = _payComponentFactory.CreateComponent(payChanel);
            basePayComponent.PrepareToPay += payservice.PrepareToPayEventHandler;
            basePayComponent.AsyncNotify += payservice.AsyncNotifyEventHandler;
            return basePayComponent;
        }

        /// <summary>
        /// 去支付
        /// </summary>
        /// <param name="serviceType">使用的业务</param>
        /// <param name="payMethod">使用的支付形式APP、H5等</param>
        /// <param name="payChanel">支付渠道：微信、支付宝等</param>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        public PayRes PrePareTpPay( string serviceType, PayMethod payMethod, PayChanel payChanel, PayReq req )
        {
            var component= BindEvent( serviceType,  payChanel);
            var type = component.GetType();
            var method = type.GetMethod(payMethod.ToString() + "Pay", new Type[] { typeof(PayReq) });
            var parameter = new object[] { req };
            var result = (PayRes)method.Invoke(component, parameter);
            return result;
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="serviceType">业务类型</param>
        /// <param name="payChanel">支付渠道：微信、支付宝等</param>
        /// <param name="context">第三方请求内容</param>
        /// <returns></returns>
        public string AsyncNotify( string serviceType, PayChanel payChanel, HttpContext context )
        {
            var component = BindEvent(serviceType, payChanel);
            return component.Notify(context);
        }

        /// <summary>
        /// 构造支付回调路径
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public string BuildPubLessonPaymentNotifyUrl( PayChanel payChannel )
        {
            var paymentNotifyUrlPath = "http://dev-webapi.rbcriyu.com/rbg/";
            switch (payChannel)
            {
                case PayChanel.AliPay:
                    return $"{paymentNotifyUrlPath}/api/H5PageNotify/Ali";
                case PayChanel.WeChatPay:
                    return $"{paymentNotifyUrlPath}/api/H5PageNotify/Wx";
                default:
                    throw new ArgumentOutOfRangeException(nameof(payChannel), payChannel, null);
            }
        }

        public ExecuteResult OrderCallback(string serviceType, string tradeNo )
        {
            var payservice = _payServices.FirstOrDefault(s => s.PayServiceType == serviceType);
            return payservice.OrderCallback(tradeNo);
        }
    }
}
