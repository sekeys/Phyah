using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phyah.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Phyah.Concurrency;
using Phyah.Web.Handler;
using Phyah.Configuration;
using Portal.Behavior;
using Phyah.TypeContainer;
using Portal.IServices;
using Portal.Services;

namespace Portal
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IHostingEnvironment env)
        {

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)

                .AddEnvironmentVariables();

            Configuration = builder.Build();

            AppSetting.AppSettings["hostdir"] = env.ContentRootPath;

            HostingEnvironment.SetRootPath(env.ContentRootPath);

            Phyah.Configuration.ConfigurationStartup.RootConfigurePath =
              System.IO.Path.Combine(env.ContentRootPath, "setting.json");

            Phyah.Configuration.ConfigurationManager.Manager.Configure();
            Phyah.Web.Initialization.Initialize();
            Phyah.Web.Initialization.InitializePipeline(new SynchronizationPipeline(
                (ex) =>
                {
                    var resp = AccessorContext.DefaultContext.Get<HttpContext>().Response;
                    resp.Clear();
                    resp.WriteAsync(ex.ToString());

                })
                .AddLast("begin", () =>
                {
                    var context = AccessorContext.DefaultContext.Get<HttpContext>();
                    var req = context.Request;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    string host = req.Host.Host;
                    if (req.Headers.ContainsKey("request-host"))
                    {
                        host = req.Headers["request-host"].ToString();
                    }
                    else if (req.Headers.ContainsKey("origin"))
                    {
                        host = req.Headers["origin"].ToString();
                    }
                    AccessorContext.DefaultContext.Set<string>("host", host);
                })
                .AddLast("initialized", new InitializedHandler())
                .AddLast("pathreset", new PathResetHandler())
                .AddLast("process", new ProcessHandler())
                );
            TypeContainer.Container.Inject(typeof(ICardService), typeof(CardService));
            TypeContainer.Container.Inject(typeof(IArticleService), typeof(ArticleService));
            TypeContainer.Container.Inject(typeof(ICardService), typeof(CardService));
            TypeContainer.Container.Inject(typeof(IPartialViewService), typeof(PartialViewService));
            TypeContainer.Container.Inject(typeof(IMenuService), typeof(MenuService));
            TypeContainer.Container.Inject(typeof(IRoleService), typeof(RoleService));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(CardsBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(ArticleBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(ContactorBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(PartialViewBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(MenuBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(RoleBehavior));
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.BufferBodyLengthLimit = int.MaxValue;
            });
            services.AddRouting();
        }

        public IConfigurationRoot Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(policy => policy
                .WithOrigins("*")
                .AllowAnyMethod()
                .WithHeaders("Access-Control-Allow-Origin, Content-Type, x-xsrf-token, Authorization,request-host,origin")
                .AllowCredentials());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = Phyah.Web.SecurityFormTicks.Schema,
                LoginPath = new PathString("/login"),
                AccessDeniedPath = new PathString("/Forbidden"),
                CookieName = ".u",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });

            app.UseMiddleware<PhyahMiddleware>();
        }
    }
}
