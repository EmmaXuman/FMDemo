using FW.Component.Pay.Event;
using FW.Models.RequestModel;
using FW.WebCore.Core;

namespace FW.Services.Pay
{
    public class H5PagePayService : BasePayService
    {
        public override string PayServiceType => "H5PagePay";
        public override PrepareToPayEventResult PrepareToPayEventHandler( PrepareToPayEventArgs args )
        {
            return new PrepareToPayEventResult();
        }

        public override AsyncNotifyEventResult AsyncNotifyEventHandler( AsyncNotifyEventArgs args )
        {
            return new AsyncNotifyEventResult();
        }

        /// <summary>
        /// 订单同步回调
        /// </summary>
        /// <param name="tradeNo"></param>
        /// <returns></returns>
        public override ExecuteResult OrderCallback( string tradeNo )
        {
            return new ExecuteResult();
        }

        public override ExecuteResult BalancePay( BalancePayReq req )
        {
            return new ExecuteResult();
        }
    }
}
