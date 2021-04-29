using FW.Common.Extensions;
using FW.Common.IDCode;
using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.Component.Pay.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WxPayAPI;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace FW.Component.Pay.Components
{
    public class WeChatComponent : BaseComponent
    {
        public override PayChanel PayChannel => PayChanel.WeChatPay;
        public override PayRes AppPay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.App);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            var wxPayData = MapToWxPayDataForApp(eventResult);
            wxPayData = TransformWxPayData(wxPayData, true);
            var signedWxPayData = SignWxPayDataForApp(wxPayData);

            return PayRes.Success(signedWxPayData);
        }

        public override PayRes H5Pay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.WXH5);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            var wxPayData = MapToWxPayDataForH5(eventResult);
            wxPayData = TransformWxPayData(wxPayData, false);

            var resData = wxPayData.GetValue("mweb_url");
            if (!string.IsNullOrEmpty(req.SyncCallbackUrl))
            {
                if (req.SyncCallbackUrl.IndexOf('?') > 0)
                {
                    req.SyncCallbackUrl += $"&out_trade_no={eventResult.TradeId}";
                }
                else
                {
                    req.SyncCallbackUrl += $"?out_trade_no={eventResult.TradeId}";
                }

                resData = $"{resData}&redirect_url={HttpUtility.UrlEncode(req.SyncCallbackUrl)}";
            }
            return PayRes.Success(resData);
        }

        public override PayRes JsPay( PayReq req )
        {
            var eventArgs = BuildPrepareToPayEventArgs(req, PayMethod.JsApi);
            var eventResult = OnPrepareToPay(eventArgs);
            if (!eventResult.Completed)
            {
                return PayRes.Fail(eventResult.Reason);
            }

            var wxPayData = MapToWxPayDataForJsApi(eventResult);
            wxPayData = TransformWxPayData(wxPayData, false);
            var signedWxPayData = SignWxPayDataForJsApi(wxPayData, eventResult.TradeId);

            return PayRes.Success(signedWxPayData);
        }

        public override string Notify( HttpContext context )
        {
            var eventArgs = BuildAsyncNotifyEventArgs(context);
            var eventResult = OnAsyncNotify(eventArgs);

            var wxPayData = new WxPayData();
            if (eventResult.Completed)
            {
                wxPayData.SetValue("return_code", "SUCCESS");
                wxPayData.SetValue("return_msg", "OK");
            }
            else
            {
                wxPayData.SetValue("return_code", "FAIL");
                wxPayData.SetValue("return_msg", "订单处理失败");
            }

            return wxPayData.ToXml();
        }

        private WxPayData MapToWxPayDataForApp( PrepareToPayEventResult eventResult )
        {
            var now = DateTime.Now;

            var wxPayData = new WxPayData();
            wxPayData.SetValue("body", eventResult.TradeTitle);
            wxPayData.SetValue("attach", eventResult.TradeDescription);
            wxPayData.SetValue("out_trade_no", eventResult.TradeId);
            wxPayData.SetValue("time_start", now.ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("time_expire", now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("goods_tag", "ONE");
            wxPayData.SetValue("total_fee", (eventResult.TradeAmount * 100).ToString("F0"));
            wxPayData.SetValue("notify_url", eventResult.OriginReq.AsyncNotifyUrl);
            wxPayData.SetValue("trade_type", "APP");

            return wxPayData;
        }

        private WxPayData MapToWxPayDataForH5( PrepareToPayEventResult eventResult )
        {
            var now = DateTime.Now;

            var wxPayData = new WxPayData();
            wxPayData.SetValue("body", eventResult.TradeTitle);
            wxPayData.SetValue("attach", eventResult.TradeDescription);
            wxPayData.SetValue("out_trade_no", eventResult.TradeId);
            wxPayData.SetValue("time_start", now.ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("time_expire", now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("total_fee", (eventResult.TradeAmount * 100).ToString("F0"));
            wxPayData.SetValue("notify_url", eventResult.OriginReq.AsyncNotifyUrl);
            wxPayData.SetValue("spbill_create_ip", eventResult.OriginReq.TerminalIp);
            wxPayData.SetValue("trade_type", "MWEB");
            wxPayData.SetValue("scene_info", "{\"h5_info\": {\"type\":\"Wap\",\"wap_url\": \"https://wap.ribencun.com\",\"wap_name\": \"日本村\"}}");

            return wxPayData;
        }

        private WxPayData MapToWxPayDataForJsApi( PrepareToPayEventResult eventResult )
        {
            var now = DateTime.Now;

            var wxPayData = new WxPayData();
            wxPayData.SetValue("body", eventResult.TradeTitle);
            wxPayData.SetValue("attach", eventResult.TradeDescription);
            wxPayData.SetValue("out_trade_no", eventResult.TradeId);
            wxPayData.SetValue("time_start", now.ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("time_expire", now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            wxPayData.SetValue("total_fee", (eventResult.TradeAmount * 100).ToString("F0"));
            wxPayData.SetValue("notify_url", eventResult.OriginReq.AsyncNotifyUrl);
            wxPayData.SetValue("spbill_create_ip", eventResult.OriginReq.TerminalIp);
            wxPayData.SetValue("trade_type", "JSAPI");
            wxPayData.SetValue("openid", eventResult.OriginReq.OpenId);

            return wxPayData;
        }

        private WxPayData TransformWxPayData( WxPayData wxPayData, bool usedByApp )
        {
            var isSandBox = false;
            #region 从数据库配置表读取当前是否测试环境
            //var serviceSysconfigResult = new SysConfigServices().GetByName("微信测试环境");
            //if (serviceSysconfigResult.Data != null)
            //{
            //    var sysConfig = (S_SysConfig)serviceSysconfigResult.Data;
            //    isSandBox = sysConfig.IsActive;
            //}
            #endregion

            if (isSandBox)
            {
                var realTotalFee = wxPayData.GetValue("total_fee");
                wxPayData.SetValue("total_fee", 1);
                wxPayData.SetValue("attach", realTotalFee);
            }

            var transformed = usedByApp ? WxPayApi.UnifiedAppOrder(wxPayData) : WxPayApi.UnifiedOrder(wxPayData);
            if (transformed.GetValue("return_code").ToString() == "SUCCESS" &&
                 transformed.GetValue("result_code").ToString() == "SUCCESS")
                return transformed;

            throw new Exception("创建微信App支付订单失败。");
        }

        private object SignWxPayDataForApp( WxPayData wxPayData )
        {
            var sortedDict = new SortedDictionary<string, object>
            {
                { "appid", wxPayData.GetValue( "appid" ) },
                { "noncestr", wxPayData.GetValue( "nonce_str" ) },
                { "package", "Sign=WXPay" },
                { "partnerid", wxPayData.GetValue( "mch_id" ) },
                { "prepayid", wxPayData.GetValue( "prepay_id" ) },
                { "timestamp", TimeExtensions.GetTimeSpan() }
            };

            var sign = PayHelper.MakeAppSign(sortedDict);

            return new
            {
                appid = sortedDict["appid"].ToString(),
                partnerid = sortedDict["partnerid"].ToString(),
                prepayid = sortedDict["prepayid"].ToString(),
                package = "Sign=WXPay",
                noncestr = sortedDict["noncestr"].ToString(),
                timestamp = sortedDict["timestamp"].ToString(),
                sign
            };
        }

        private object SignWxPayDataForJsApi( WxPayData wxPayData, string tradeId )
        {
            var sortedDict = new SortedDictionary<string, object>
            {
                { "appId", wxPayData.GetValue( "appid" ) },
                { "nonceStr", wxPayData.GetValue( "nonce_str" ) },
                { "package", $"prepay_id={wxPayData.GetValue( "prepay_id" )}" },
                { "timeStamp", TimeExtensions.GetTimeSpan() },
                { "signType", "MD5" }
            };

            var paySign = PayHelper.MakeSign(sortedDict);

            return new
            {
                appid = sortedDict["appId"].ToString(),
                package = sortedDict["package"],
                noncestr = sortedDict["nonceStr"].ToString(),
                timestamp = sortedDict["timeStamp"].ToString(),
                signType = sortedDict["signType"].ToString(),
                paySign,
                out_trade_no = tradeId
            };
        }

        private PrepareToPayEventArgs BuildPrepareToPayEventArgs( PayReq req, PayMethod method )
        {
            var eventArgs = new PrepareToPayEventArgs
            {
                OrderId = req.LocalOrderId,
                PayChannel = PayChanel.WeChatPay,
                PayMethod = method,
                OriginReq = req
            };

            return eventArgs;
        }

        private AsyncNotifyEventArgs BuildAsyncNotifyEventArgs( HttpContext context )
        {
            string notifyData;

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                notifyData = reader.ReadToEnd();
            }

            var wxPayData = new WxPayData();
            wxPayData.FromXml(notifyData);

            if (!wxPayData.IsSet("result_code"))
            {

                throw new Exception("微信异步通知结果缺少必要字段。");
            }

            #region 查询数据库配置是否测试环境
            var isSandBox = false;
            //var serviceSysconfigResult = new SysConfigServices().GetByName("微信测试环境");
            //if (serviceSysconfigResult.Data != null)
            //{
            //    var sysConfig = (S_SysConfig)serviceSysconfigResult.Data;
            //    isSandBox = sysConfig.IsActive;
            //}
            #endregion
            var totalFee = isSandBox ? wxPayData.GetValue("attach").ToString() : wxPayData.GetValue("total_fee").ToString();

            var tradeAmount = decimal.Parse(totalFee);
            tradeAmount = decimal.Round(tradeAmount / 100, 2);

            var eventArgs = new AsyncNotifyEventArgs
            {
                Channel = PayChanel.WeChatPay,
                Completed = wxPayData.GetValue("result_code").ToString() == "SUCCESS",
                TradeId = wxPayData.GetValue("out_trade_no").ToString(),
                TradeSpId = wxPayData.GetValue("transaction_id").ToString(),
                TradeAmount = tradeAmount
            };

            return eventArgs;
        }
    }
}
