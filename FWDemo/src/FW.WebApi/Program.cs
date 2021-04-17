using Autofac.Extensions.DependencyInjection;
using FW.DbContexts;
using FW.UintOfWork.UnitOfWork;
using FW.WebApi.Initialiaze;
using FW.WebCore.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;

namespace FW.WebApi
{
    public class Program
    {
        public static void Main( string[] args )
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                    NLogExtensions.EnsureNlogConfig("NLog.config", "MySQL", scope.ServiceProvider.GetRequiredService<IConfiguration>().GetSection("ConnectionStrings:MSDbContext").Value);
                    //��ʼ�����ݿ�
                    DBSeed.Initialize(scope.ServiceProvider.GetRequiredService<IUnitOfWork<MSDbContext>>());
                }
                logger.Trace("��վ�������");
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "��վ����ʧ��");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())//�滻autofac��ΪDI����
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).AddNlogService();
    }
}
