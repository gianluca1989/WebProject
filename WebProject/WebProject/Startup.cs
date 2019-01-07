using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using WebProject.EF;
using WebProject.EF.UoW;
using WebProject.EFInterface;
using WebProject.EFInterface.UoW;
using WebProject.Entity;
using WebProject.Autentication;
using WebProject.Utils;
using log4net;
using System.Reflection;

namespace WebProject
{
    public class Startup
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            try
            {
                //Configure culture, in this case the sect as en-US
                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") };
                    options.RequestCultureProviders.Clear();
                });



                services.AddTransient<IOrderRepository, EFOrderRepository>();
                services.AddTransient<IOrderUnitOfWork, EFOrderUnitOfWork>();

                //inject the component into the interface for authentication
                services.AddIdentity<StoreUser, IdentityRole>(cfg =>//per autenticazione
                {
                    cfg.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<MyContext>();

                // Cookie settings
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/Login";
                    options.SlidingExpiration = true;
                });

                services.AddAuthentication();


                //takes the connection string from the configuration file
                string a = new Configuration().GetField("ConnectionStrings:DefaultConnection");

                services.AddDbContext<MyContext>(opts => opts.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


                services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

                // Add Kendo UI services to the services container
                services.AddKendo();
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
                throw new Exception(e.Message);
            }
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            try
            {
                //for add new culture
                app.UseRequestLocalization();

                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBrowserLink();
                    app.UseDatabaseErrorPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseStaticFiles();
                app.UseAuthentication();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Users}/{action=Login}/{id?}");
                });
                // Configure Kendo UI
                app.UseKendo(env);
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
                throw new Exception(e.Message);
            }
            
        }
    }
}
