using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Event
{
    public class PrepareToPayEventArgs
    {
        /// <summary>
        /// 需要发起支付的订单
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 发起支付的渠道
        /// </summary>
        public PayChanel PayChannel { get; set; }
        /// <summary>
        /// 发起支付的方式
        /// </summary>
        public PayMethod PayMethod { get; set; }

        /// <summary>
        /// 来源请求参数
        /// </summary>
        public PayReq OriginReq { get; set; }
    }
}
