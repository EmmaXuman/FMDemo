using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Component.Pay.Event
{
    /// <summary>
    /// 事件结果
    /// </summary>
    public class EventResult
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Completed { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
}
