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
            _logger = LogManager.GetLogger(Logger.Name, typeof(Startup));
        }

        public static ILoggerRepository Logger { get; set; }

        static ILog _logger;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ÆôÓÃ¿ØÖÆÆ÷
            services.AddControllers();
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(SysExceptionFilter));
                //option.EnableEndpointRouting = false;
            });
            services.AddRazorPages();
            services.AddSignalR().AddNewtonsoftJsonProtocol();
            //ÅäÖÃ¿çÓò
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
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
           // loggerFactory.AddLog4Net();

            _logger.Info("Startpup");
            _logger.Info("Startpup2");

            {
                var basePath = Directory.GetCurrentDirectory() + "\\logs\\system.log";
                var fs = new FileStream(basePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var reader = new StreamReader(fs);
                var json = reader.ReadToEnd();
                reader.Close();
                fs.Close();
                //File.Delete(basePath);
                var str = json.Replace("\r\n", "");
                var arr = str.Split('|');
                var arr2 = arr.Where(w => !string.IsNullOrEmpty(w));
                foreach (var item in arr2)
                {
                    var first = item.Split(',');
                    LogResut resut = new LogResut();
                    var xx = first[0].Split('£º')[1];
                    resut.CreateTime = Convert.ToDateTime(first[0].Split('£º')[1]);
                    resut.Level = first[2].Split('£º')[1];
                    resut.Summary = first[3].Split('£º')[1];
                }
                int length = str.Length;
                var sub = str.Remove(length - 1);
                var result = "[" + sub + "]";
            }
            app.UseStaticFiles();
            
            //ÅäÖÃ¿çÓò
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //ÖÕ½áµãÉèÖÃÂ·ÓÉÄ¬ÈÏ
                endpoints.MapControllerRoute(
                               name: "default",
                               pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
    public class LogResut
    {
        public DateTime CreateTime { get; set; }
        public string Level { get; set; }
        public string Summary { get; set; }
    }
}
