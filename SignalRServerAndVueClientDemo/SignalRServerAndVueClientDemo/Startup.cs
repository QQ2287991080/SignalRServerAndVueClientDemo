using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRServerAndVueClientDemo.Filters;
using SignalRServerAndVueClientDemo.Hubs;

namespace SignalRServerAndVueClientDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Logger = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(Logger, new FileInfo("log4net.config"));
        }

        public static ILoggerRepository Logger { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //启用控制器
            services.AddControllers();
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(SysExceptionFilter));
                //option.EnableEndpointRouting = false;
            });
            services.AddRazorPages();
            services.AddSignalR().AddNewtonsoftJsonProtocol();
            //配置跨域
            services.AddCors(c =>
                c.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                })
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,LoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //log4 extension
            loggerFactory.AddLog4Net();

            app.UseStaticFiles();
            
            //配置跨域
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //终结点设置路由默认
                endpoints.MapControllerRoute(
                               name: "default",
                               pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
