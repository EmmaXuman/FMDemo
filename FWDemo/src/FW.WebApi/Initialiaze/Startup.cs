using Autofac;
using FW.Compoment.Jwt;
using FW.Component.Aop;
using FW.DbContexts;
using FW.Models.Automapper;
using FW.Services;
using FW.UintOfWork;
using FW.WebApi.Fileters;
using FW.WebApi.Filters;
using FW.WebCore;
using FW.WebCore.MultiLanguages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FW.WebApi
{
    public class Startup
    {
        //public Startup( IConfiguration configuration )
        //{
        //    Configuration = configuration;
        //}
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        //添加autofac的DI配置容器
        public void ConfigureContainer( ContainerBuilder builder )
        {
            //注册IBaseService和IRoleService接口及对应的实现类
            //builder.RegisterServices();
            builder.AddAopService(ServiceExtensions.GetAssemblyName());
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMultiLanguages();

            //services.AddControllers();
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<ApiExceptionFilter>();
            }).AddDataAnnotationsLocalization(options=>
            {
                options.DataAnnotationLocalizerProvider = ( type, factory ) =>
                  factory.Create(typeof(SharedResource));
            });

            services.AddUnitOfWorkService<MSDbContext>(options=> { options.UseMySql(Configuration.GetSection("ConnectionStrings:MSDbContext").Value); });

            //using MS.WebCore;
            //添加以上代码至using引用
            //注册跨域策略
            services.AddCorsPolicy(Configuration);
            //注册webcore服务（网站主要配置）
            services.AddWebCoreService(Configuration);
            //注册automapper服务
            services.AddAutomapperService();
            //注册Service类库中的服务
            //services.AddServiceExtention();

            //注册jwt服务
            services.AddJwtService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultiLanguage(Configuration);//添加多语言本地化支持

            app.UseRouting();

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//添加跨域

            app.UseAuthentication();//添加认证中间件

            app.UseAuthorization();//授权中间件，中间件的顺序不能随意调整

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
