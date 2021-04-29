
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using FW.WebCore.Core;
using System;

namespace FW.Component.Pay.Config
{
    /// <summary>
    /// 支付相关公共类
    /// </summary>
    public class PaymentCommon
    {
        #region 支付宝正式环境 AlipayClint
        /// <summary>
        /// 支付宝正式环境
        /// </summary>
        /// <returns></returns>
        public static DefaultAopClient AlipayClint()
        {
            DefaultAopClient client = new DefaultAopClient(AlipaySdkConfig.gatewayUrl, AlipaySdkConfig.app_id, AlipaySdkConfig.private_key, "json", "1.0", AlipaySdkConfig.sign_type, AlipaySdkConfig.alipay_public_key, AlipaySdkConfig.charset, false);
            return client;
        }
        #endregion

        #region 支付宝测试环境 AlipaySandClint
        /// <summary>
        /// 支付宝测试环境
        /// </summary>
        /// <returns></returns>
        public static DefaultAopClient AlipaySandClint()
        {
            DefaultAopClient client = new DefaultAopClient(AlipaySandBoxSdkConfig.gatewayUrl, AlipaySandBoxSdkConfig.app_id, AlipaySandBoxSdkConfig.private_key, "json", "1.0", AlipaySandBoxSdkConfig.sign_type, AlipaySandBoxSdkConfig.alipay_public_key, AlipaySandBoxSdkConfig.charset, false);
            return client;
        }
        #endregion

        #region PC端----支付宝支付提交请求，返回跳转主体 GetAlipayResponse
        /// <summary>
        /// 支付宝支付提交请求，返回跳转主体
        /// </summary>
        /// <param name="return_url">同步回发地址</param>
        /// <param name="notify_url">异步回发地址</param>
        /// <param name="model">支付宝构造参数</param>
        /// <param name="client">支付宝构造请求参数</param>
        /// <returns>申请成功或失败的集合 Body</returns>
        public static ExecuteResult GetAlipayResponse( string return_url, string notify_url, AlipayTradePagePayModel model, DefaultAopClient client )
        {

            var sr = new ExecuteResult<string>();
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(return_url);
            // 设置异步通知接收地址
            request.SetNotifyUrl(notify_url);
            // 将业务model载入到request
            request.SetBizModel(model);
            AlipayTradePagePayResponse response = null;
            try
            {
                response = client.pageExecute(request, null, "post");
                //保存流水号信息
                sr.Set(true, "支付宝请求成功", response.Body);
            }
            catch (Exception exp)
            {
                sr.Set(false, "支付宝请求失败");
            }
            return sr;
        }
        #endregion

        #region APP端----支付宝支付提交请求，返回跳转主体 GetAlipayResponse
        /// <summary>
        /// 支付宝支付提交请求，返回跳转主体
        /// </summary>
        /// <param name="return_url">同步回发地址</param>
        /// <param name="notify_url">异步回发地址</param>
        /// <param name="model">支付宝构造参数</param>
        /// <param name="client">支付宝构造请求参数</param>
        /// <returns>申请成功或失败的集合 Body</returns>
        public static ExecuteResult GetAlipayResponse( string return_url, string notify_url, AlipayTradeAppPayModel model, DefaultAopClient client )
        {

            var sr = new ExecuteResult<string>();
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(return_url);
            // 设置异步通知接收地址
            request.SetNotifyUrl(notify_url);
            // 将业务model载入到request
            request.SetBizModel(model);
            AlipayTradeAppPayResponse response = null;
            try
            {
                response = client.SdkExecute(request);
                //保存流水号信息
                sr.Set(true, "支付宝APP请求成功", response.Body);
            }
            catch (Exception exp)
            {
                sr.Set(false, "支付宝APP请求失败");
            }
            return sr;
        }
        #endregion

    }
}
