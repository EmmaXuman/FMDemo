using Castle.Core.Logging;
using FW.Component.Aop.LogAop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FW.WebApi.Fileters
{
    /// <summary>
    /// api异常过滤器
    /// </summary>
    public class ApiExceptionFilter:IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string methodInfo = $"{context.RouteData.Values["controller"] as string}Controller.{context.RouteData.Values["action"] as string}:{context.HttpContext.Request.Method}";

            if (!(context.Exception is AopHandledException))
            {
                _logger.LogError(context.Exception, "执行{0}时发生错误！", methodInfo);
            }
            context.Result = new JsonResult(new
            {
                status=501,
                data="服务器出错"
            });
        }
    }
}
