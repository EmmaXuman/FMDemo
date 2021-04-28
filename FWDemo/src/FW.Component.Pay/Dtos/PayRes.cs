using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Dtos
{
    /// <summary>
    /// 发起支付的结果 Dto:数据传输对象
    /// </summary>
    public class PayRes
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public static PayRes Success<T>( T data )
        {
            return new PayRes
            {
                IsSuccess = true,
                Msg = string.Empty,
                DataType = typeof(T),
                Data = data
            };
        }

        public static PayRes Fail( string msg )
        {
            return new PayRes
            {
                IsSuccess = false,
                Msg = msg,
                DataType = null,
                Data = null
            };
        }
    }
}
