using FW.Component.Pay.Event;

namespace FW.Services.Pay
{
    public class H5PayService : BasePayService
    {
        public override string PayServiceType => "H5Pay";
        public override PrepareToPayEventResult PrepareToPayEventHandler( PrepareToPayEventArgs args )
        {
            return new PrepareToPayEventResult();
        }

        public override AsyncNotifyEventResult AsyncNotifyEventHandler( AsyncNotifyEventArgs args )
        {
            return new AsyncNotifyEventResult();
        }

    }
}
