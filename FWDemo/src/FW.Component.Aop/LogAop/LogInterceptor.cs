using Castle.DynamicProxy;

namespace FW.Component.Aop.LogAop
{
    public class LogInterceptor : IInterceptor
    {
        private readonly LogInterceptorAsync _logInterceptorAsync;
        public void Intercept( IInvocation invocation )
        {
            _logInterceptorAsync.ToInterceptor().Intercept(invocation);
        }

        public LogInterceptor( LogInterceptorAsync logInterceptorAsync )
        {
            _logInterceptorAsync = logInterceptorAsync;
        }
    }
}
