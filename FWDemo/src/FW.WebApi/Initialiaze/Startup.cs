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
        //���autofac��DI��������
        public void ConfigureContainer( ContainerBuilder builder )
        {
            //ע��IBaseService��IRoleService�ӿڼ���Ӧ��ʵ����
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
            //������ϴ�����using����
            //ע��������
            services.AddCorsPolicy(Configuration);
            //ע��webcore������վ��Ҫ���ã�
            services.AddWebCoreService(Configuration);
            //ע��automapper����
            services.AddAutomapperService();
            //ע��Service����еķ���
            //services.AddServiceExtention();

            //ע��jwt����
            services.AddJwtService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultiLanguage(Configuration);//��Ӷ����Ա��ػ�֧��

            app.UseRouting();

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//��ӿ���

            app.UseAuthentication();//�����֤�м��

            app.UseAuthorization();//��Ȩ�м�����м����˳�����������

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
