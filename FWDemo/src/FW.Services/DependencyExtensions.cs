using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace FW.Services
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddServiceExtention( this IServiceCollection services )
        {
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }

        public static void RegisterServices( this ContainerBuilder builder )
        {
            builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
        }

    }
}
