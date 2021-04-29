using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Config
{
     public class AlipaySdkConfig
    {
        public AlipaySdkConfig()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #region 正式环境配置
        // 应用ID,您的APPID
        public static string app_id = "";
        //// 支付宝网关
        public static string gatewayUrl = "";
        //// 商户私钥，您的原始格式RSA私钥
        public static string private_key = "";
        //// 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key = "";
        #endregion

        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";
    }
}
