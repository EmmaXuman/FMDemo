using FW.Common.Extensions;
using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.Component.Pay.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FW.Component.Pay.Components
{
    public class ApplePayComponent : BaseComponent
    {
        public override PayChanel PayChannel => PayChanel.ApplePay;
        private const string VERIFY_URL = "https://buy.itunes.apple.com/verifyReceipt";
        private const string VERIRY_URL_SANDBOX = "https://sandbox.itunes.apple.com/verifyReceipt";
        private readonly IConfiguration _configuration;

        public ApplePayComponent( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public override PayRes AppPay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.App);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            return PayRes.Success(new
            {
                eventResult.TradeId,
                ProductId = "6"
            });
        }

        public override string Notify( HttpContext context )
        {
            var eventArgs = BuildAsyncNotifyEventArgs(context);
            var eventResult = OnAsyncNotify(eventArgs);

            return eventResult.Completed ? string.Empty : eventResult.Reason;
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

        // See https://developer.apple.com/documentation/appstorereceipts/verifyreceipt for more information.
        private AsyncNotifyEventArgs BuildAsyncNotifyEventArgs( HttpContext context )
        {
            string notifyData;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                notifyData = reader.ReadToEnd();
            }

            var json = JObject.Parse(notifyData);
            if (!json.ContainsKey("ReceiptData"))
            {
                throw new Exception("苹果内购通知数据不合法。");
            }

            var verifyData = $"{{\"receipt-data\":\"{json.Value<string>("ReceiptData")}\"}}";
            var postUrl =  _configuration.GetSection("AppleVerify").Value == "1" ? VERIFY_URL : VERIRY_URL_SANDBOX;
            var verifyResult = PayHelper.Post(postUrl, verifyData);
            var verifyJson = JObject.Parse(verifyResult);
            var status = verifyJson.Value<int>("status");

            var eventArgs = new AsyncNotifyEventArgs
            {
                Channel = PayChanel.ApplePay,
                Completed = status == 0,
                TradeId = json.Value<string>("TradeId"),
                TradeSpId = GetTranscationIdFromVerifyJson(verifyJson),
                TradeAmount = json.Value<decimal>("TradeAmount")
            };

            return eventArgs;
        }

        private static string GetTranscationIdFromVerifyJson( JObject json )
        {
            if (!json.TryGetValue("receipt", StringComparison.OrdinalIgnoreCase, out var jsonReceipt))
            {
                return string.Empty;
            }

            var jsonInApp = jsonReceipt["in_app"];
            var jsonFirst = jsonInApp?.First;
            return jsonFirst?["transaction_id"]?.ToString() ?? string.Empty;
        }
    }
}
