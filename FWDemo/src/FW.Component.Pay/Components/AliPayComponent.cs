using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Util;
using FW.Component.Pay.Config;
using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.Component.Pay.Event;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FW.Component.Pay.Components
{
    public class AliPayComponent : BaseComponent
    {
        public override PayChanel PayChannel => PayChanel.AliPay;

        public override PayRes AppPay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.App);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            var appPayRequest = BuildAppPayRequest(eventResult);
            var result = Client().SdkExecute(appPayRequest);

            return PayRes.Success(result);
        }

        public override PayRes H5Pay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.AliH5);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            var appPayRequest = BuildH5PayRequest(eventResult);
            var result = Client().pageExecute(appPayRequest);

            return PayRes.Success(result.Body);
        }

        public override string Notify( HttpContext context )
        {
            var eventArgs = BuildAsyncNotifyEventArgs(context);
            var eventResult = OnAsyncNotify(eventArgs);

            return eventResult.Completed ? "success" : string.Empty;
        }

        private DefaultAopClient Client()
        {
            var isSandBox = false;
            //var serviceSysconfigResult = new SysConfigServices().GetByName("支付宝测试环境");
            //if (serviceSysconfigResult.Data != null)
            //{
            //    var sysConfig = (S_SysConfig)serviceSysconfigResult.Data;
            //    isSandBox = sysConfig.IsActive;
            //}

            return isSandBox ? PaymentCommon.AlipaySandClint() : PaymentCommon.AlipayClint();
        }

        private AlipayTradeAppPayRequest BuildAppPayRequest( PrepareToPayEventResult eventResult )
        {
            var appPayBizModel = new AlipayTradeAppPayModel
            {
                OutTradeNo = eventResult.TradeId,
                TotalAmount = eventResult.TradeAmount.ToString("F2"),
                Body = eventResult.TradeDescription,
                Subject = eventResult.TradeTitle,
                ProductCode = "QUICK_MSECURITY_PAY",
                TimeoutExpress = "30m"
            };

            var appPayRequest = new AlipayTradeAppPayRequest();
            appPayRequest.SetReturnUrl(eventResult.OriginReq.SyncCallbackUrl);
            appPayRequest.SetNotifyUrl(eventResult.OriginReq.AsyncNotifyUrl);
            appPayRequest.SetBizModel(appPayBizModel);

            return appPayRequest;
        }

        private AlipayTradeWapPayRequest BuildH5PayRequest( PrepareToPayEventResult eventResult )
        {
            var appPayBizModel = new AlipayTradeWapPayModel
            {
                OutTradeNo = eventResult.TradeId,
                TotalAmount = eventResult.TradeAmount.ToString("F2"),
                Body = eventResult.TradeDescription,
                Subject = eventResult.TradeTitle,
                ProductCode = "QUICK_WAP_PAY",
                TimeoutExpress = "30m"
            };

            var appPayRequest = new AlipayTradeWapPayRequest();
            appPayRequest.SetReturnUrl(eventResult.OriginReq.SyncCallbackUrl);
            appPayRequest.SetNotifyUrl(eventResult.OriginReq.AsyncNotifyUrl);
            appPayRequest.SetBizModel(appPayBizModel);

            return appPayRequest;
        }

        private PrepareToPayEventArgs BuildPrepareToPayEventArgs( PayReq req, PayMethod method )
        {
            var eventArgs = new PrepareToPayEventArgs
            {
                OrderId = req.LocalOrderId,
                PayChannel = PayChanel.AliPay,
                PayMethod = method,
                OriginReq = req
            };

            return eventArgs;
        }

        private AsyncNotifyEventArgs BuildAsyncNotifyEventArgs( HttpContext context )
        {
            var form = context.Request.Form;
            var notifyData = new Dictionary<string, string>();
            foreach (var key in form.Keys)
            {
                notifyData.Add(key, form[key]);
            }

            if (notifyData.Count <= 0)
            {
                throw new Exception("支付宝异步回调没有获取到任何有效数据。");
            }

            var appId = notifyData["app_id"];
            var isValid = AlipaySignature.RSACheckV1(notifyData, AlipaySdkConfig.alipay_public_key, AlipaySdkConfig.charset, AlipaySdkConfig.sign_type, false);

            var isSandBox = false;
            //var serviceSysconfigResult = new SysConfigServices().GetByName("支付宝测试环境");
            //if (serviceSysconfigResult.Data != null)
            //{
            //    var sysConfig = (S_SysConfig)serviceSysconfigResult.Data;
            //    isSandBox = sysConfig.IsActive;
            //}

            if (!isValid && !isSandBox)
            {
                throw new Exception("支付宝异步回调参数校验失败。");
            }

            var rightAppId = isSandBox ? AlipaySandBoxSdkConfig.app_id : AlipaySdkConfig.app_id;
            var isRight = rightAppId == appId;

            if (!isRight)
            {
                throw new Exception("支付宝异步回调参数不匹配。");
            }

            var eventArgs = new AsyncNotifyEventArgs
            {
                Channel = PayChanel.AliPay,
                Completed = notifyData["trade_status"] == "TRADE_SUCCESS",
                TradeId = notifyData["out_trade_no"],
                TradeSpId = notifyData["trade_no"],
                TradeAmount = decimal.Parse(notifyData["total_amount"])
            };

            return eventArgs;
        }
    }
}
