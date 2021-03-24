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
            //������ϴ�����using����
            //ע��������
            services.AddCorsPolicy(Configuration);
            //ע��webcore������վ��Ҫ���ã�
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

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//��ӿ���

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
