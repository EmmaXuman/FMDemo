using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Enums
{
    /// <summary>
    /// 支付渠道
    /// !!! 支付渠道的枚举值应该与继承 PayComponent 的 xxxComponent 类型的 xxx 名字一致 !!!
    /// </summary>
    public enum PayChanel
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        AliPay = 1001,
        /// <summary>
        /// 微信支付
        /// </summary>
        WeChatPay = 1002,
        /// <summary>
        /// 苹果内购
        /// </summary>
        ApplePay = 1003,
    }
}
