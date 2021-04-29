using FW.Component.Pay.Dtos;
using FW.Component.Pay.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FW.Component.Pay
{
    public class PayComponentFactory
    {
        private readonly IEnumerable<BaseComponent> _components;
        private BaseComponent component;
        public PayComponentFactory( IEnumerable<BaseComponent> components )
        {
            _components = components;
        }

        public BaseComponent CreateComponent( PayChanel payChanel )
        {
            return _components.FirstOrDefault(f => f.PayChannel == payChanel);
        }
    }
}
