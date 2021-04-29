using FW.Component.Pay.Event;
using FW.Models.RequestModel;
using FW.WebCore.Core;
using System;

namespace FW.Services.Pay
{
    public abstract class BasePayService
    {
        public abstract string PayServiceType { get; }
        public abstract PrepareToPayEventResult PrepareToPayEventHandler( PrepareToPayEventArgs args );

        public abstract AsyncNotifyEventResult AsyncNotifyEventHandler( AsyncNotifyEventArgs args );

        public virtual ExecuteResult BalancePay( BalancePayReq req )
        {
            throw new NotImplementedException();
        }
        public virtual ExecuteResult OrderCallback( string tradeNo )
        {
            throw new NotImplementedException();
        }
    }
}
