using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Dtos
{
    /// <summary>
    /// 支付参数
    /// </summary>
    public class PayReq
    {
        /// <summary>
        /// 本地需要发起支付的订单ID
        /// </summary>
        public string LocalOrderId { get; set; }
        /// <summary>
        /// 发起支付的终端设备的 IP 地址
        /// </summary>
        public string TerminalIp { get; set; }
        /// <summary>
        /// 微信支付的 OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 同步回调地址
        /// </summary>
        public string SyncCallbackUrl { get; set; }
        /// <summary>
        /// 异步回调地址
        /// </summary>
        public string AsyncNotifyUrl { get; set; }
        /// <summary>
        /// 支付总金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 区分类型： 1购买，2定金，3充值 4退款 5多笔支付 6余额  0未确定
        /// </summary>
        public int Type { get; set; }
    }
}
