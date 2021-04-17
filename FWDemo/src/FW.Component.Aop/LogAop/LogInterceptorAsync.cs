using Castle.DynamicProxy;
using FW.Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FW.Component.Aop.LogAop
{
    public class LogInterceptorAsync : IAsyncInterceptor
    {
        private readonly ILogger<LogInterceptorAsync> _logger;

        public LogInterceptorAsync( ILogger<LogInterceptorAsync> logger )
        {
            _logger = logger;
        }
        /// <summary>
        /// t同步方法拦截时使用
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous( IInvocation invocation )
        {
            try
            {
                //调用业务方法
                invocation.Proceed();
                LogExecuteInfo(invocation, invocation.ReturnValue.ToJsonString());//记录日志
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation);
                throw new AopHandledException();
            }
        }
        /// <summary>
        /// 异步方法返回task时使用
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous( IInvocation invocation )
        {
            try
            {
                //调用业务方法
                invocation.Proceed();
                LogExecuteInfo(invocation, invocation.ReturnValue.ToJsonString());//koi日志
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation);
                throw new AopHandledException();
            }
        }

        /// <summary>
        /// 异步方法返回Task<T>时使用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous<TResult>( IInvocation invocation )
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>( IInvocation invocation )
        {
            try
            {
                //调用业务方法
                invocation.Proceed();
                Task<TResult> task = (Task<TResult>)invocation.ReturnValue;
                TResult result = await task;//获得返回结果
                LogExecuteInfo(invocation, result.ToJsonString());

                return result;
            }
            catch (Exception ex)
            {
                LogExecuteError(ex, invocation);
                throw new AopHandledException();
            }
        }


        #region Helpmethod
        /// <summary>
        /// 获取拦截方法信息（类型、方法名、参数）
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GetMethodIndo( IInvocation invocation )
        {
            //方法类名
            string className = invocation.Method.DeclaringType.Name;
            //方法名
            string methodName = invocation.Method.Name;
            //参数
            string args = string.Join(",", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());
            if (string.IsNullOrWhiteSpace(args))
            {
                return $"{className}.{methodName}";
            }
            else
            {
                return $"{className}.{methodName}:{args}";
            }
        }

        private void LogExecuteInfo( IInvocation invocation, string result )
        {
            _logger.LogDebug("方法{0},返回值{1}", GetMethodIndo(invocation), result);
        }
        private void LogExecuteError( Exception ex, IInvocation invocation )
        {
            _logger.LogError(ex, "实行{0}时发生错误！", GetMethodIndo(invocation));
        }
        #endregion
    }
}
