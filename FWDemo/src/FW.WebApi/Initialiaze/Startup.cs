using FW.DbContexts;
using FW.UintOfWork;
using FW.WebCore;
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
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllers();
            var connStr = Configuration.GetSection("ConnectionStrings:MSDbContext").Value;
            services.AddUnitOfWorkService<MSDbContext>(options=> { options.UseMySql(Configuration.GetSection("ConnectionStrings:MSDbContext").Value); });

            //using MS.WebCore;
            //添加以上代码至using引用
            //注册跨域策略
            services.AddCorsPolicy(Configuration);
            //注册webcore服务（网站主要配置）
            services.AddWebCoreService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//添加跨域

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
