using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Enums
{
    public enum PayMethod
    {
        /// <summary>
        /// App支付
        /// </summary>
        App = 1001,
        /// <summary>
        /// 手机 H5 页面支付
        /// </summary>
        H5 = 1002,
        /// <summary>
        /// 微信 JS-API 支付
        /// </summary>
        JsApi = 1003,
        /// <summary>
        /// 微信h5
        /// </summary>
        WXH5 = 1004,
        /// <summary>
        /// 阿里h5
        /// </summary>
        AliH5 = 1005,
    }
}
