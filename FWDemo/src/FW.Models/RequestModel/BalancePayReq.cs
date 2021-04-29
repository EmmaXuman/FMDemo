using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Models.RequestModel
{
    /// <summary>
    /// 余额支付请求参数
    /// </summary>
   public class BalancePayReq
    {
        public int OrderId { get; set; }
        public int Balance { get; set; }
        public int UserInfoId { get; set; }
    }
}
