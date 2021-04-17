using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FW.Services
{
    public static class ServiceExtensions
    {
        //public static IServiceCollection AddServiceExtention( this IServiceCollection services )
        //{
        //    services.AddScoped<IBaseService, BaseService>();
        //    services.AddScoped<IRoleService, RoleService>();
        //    return services;
        //}

        //public static void RegisterServices( this ContainerBuilder builder )
        //{
        //    builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();
        //    builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
        //}
        /// <summary>
        /// 获取程序集名称 提供给Autofac进行批量的注册接口和实现
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}
