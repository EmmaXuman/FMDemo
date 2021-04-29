using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Models.RequestModel
{
    public class H5PageAliPayReq
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 前端返回地址
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 区分类型： 1购买，2定金，3充值 4退款 5多笔支付 6余额  0未确定
        /// </summary>
        public int Type { get; set; }
    }
}
