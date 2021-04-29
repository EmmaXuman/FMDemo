using System.ComponentModel;

namespace FW.Component.Pay.Config
{
    [Description("支付沙箱环境")]
    public class AlipaySandBoxSdkConfig
    {
        public AlipaySandBoxSdkConfig()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 测试环境配置

        //测试
        public static string app_id = "2016082100303812";
        // 测试支付宝网关
        public static string gatewayUrl = "https://openapi.alipaydev.com/gateway.do";
        // 商户私钥，您的原始格式RSA私钥
        public static string private_key = "";

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key = "";
        #endregion



        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";
    }
}
