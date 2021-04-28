using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Event
{
    public class AsyncNotifyEventArgs
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string TradeId { get; set; }
        /// <summary>
        /// 服务商交易编号
        /// </summary>
        public string TradeSpId { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 支付渠道
        /// </summary>
        public PayChannel Channel { get; set; }
        /// <summary>
        /// 支付是否完成
        /// </summary>
        public bool Completed { get; set; }
    }
}
