using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using FW.Component.Pay.Event;
using Microsoft.AspNetCore.Http;
using System;

namespace FW.Component.Pay
{
    /// <summary>
    /// 支付组件
    /// </summary>
    public abstract class BaseComponent
    {
        public abstract PayChanel PayChannel { get; }
        /// <summary>
        /// 准备发起支付事件
        /// </summary>
        public event Func<PrepareToPayEventArgs, PrepareToPayEventResult> PrepareToPay;
        /// <summary>
        /// 异步回调事件
        /// </summary>
        public event Func<AsyncNotifyEventArgs, AsyncNotifyEventResult> AsyncNotify;

        /// <summary>
        /// APP支付
        /// </summary>
        /// <returns></returns>
        public virtual PayRes AppPay( PayReq req )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// H5支付
        /// </summary>
        /// <returns></returns>
        public virtual PayRes H5Pay( PayReq req )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Js-Api 支付
        /// </summary>
        /// <returns></returns>
        public virtual PayRes JsPay( PayReq req )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract string Notify( HttpContext context );


        protected PrepareToPayEventResult OnPrepareToPay( PrepareToPayEventArgs args )
        {
            if (PrepareToPay == null)
            {
                throw new NotImplementedException("Couldn't call PrepareToPay, because it isn't binding.");
            }

            return PrepareToPay(args);
        }

        protected AsyncNotifyEventResult OnAsyncNotify( AsyncNotifyEventArgs args )
        {
            if (AsyncNotify == null)
            {
                throw new NotImplementedException("Couldn't call OnAsyncNotify, because it isn't binding.");
            }

            return AsyncNotify(args);
        }
    }
}
