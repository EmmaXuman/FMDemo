using FW.Component.Pay.Dtos;

namespace FW.Component.Pay.Event
{
    public class PrepareToPayEventResult : EventResult
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 交易标题
        /// </summary>
        public string TradeTitle { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        public string TradeDescription { get; set; }

        /// <summary>
        /// 来源请求参数
        /// </summary>
        public PayReq OriginReq { get; set; }
    }
}
