using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phyah.Configuration;
using Phyah.Web;
using Microsoft.Extensions.Configuration;
using Phyah.Concurrency;
using Phyah.Web.Handler;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Phyah.Huaxue;

namespace JxHuaxue
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
                .AddLast(new InitializedHandler())
                .AddLast(new PathResetHandler())
                .AddLast(new ProcessHandler())
                );
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(IndexBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(DashbordBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(UserBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(UserListBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(CardsBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(CardListBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(ModuleBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(ModuleListBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(PageListBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(PageBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(PageModulesBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(UploadBehavior));
            Phyah.Web.BehaviorFactory.Factory.Cache(typeof(RazorBehavior));
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

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath, "wwwroot","js")),
                RequestPath = new PathString("/js")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath, @"wwwroot", "css")),
                RequestPath = new PathString("/css")
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath, @"statichtml")),
                RequestPath = new PathString("/statichtml")
            });
            //app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath, @"assets")),
            //    RequestPath = new PathString("/assets")
            //});
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = Phyah.Web.SecurityFormTicks.Schema,
                LoginPath = new PathString("/login"),
                AccessDeniedPath = new PathString("/Forbidden"),
                CookieName = ".u",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });

            //app.UseRouter(routes =>
            //{
            //    routes.DefaultHandler = new RouteHandler((httpContext) =>
            //    {
            //        var request = httpContext.Request;
            //        return httpContext.Response.WriteAsync($"Verb =  {request.Method.ToUpperInvariant()} - Path = {request.Path} - Route values - {string.Join(", ", httpContext.GetRouteData().Values)}");
            //    });

            //    routes
            //    .MapGet("api/get/{id}", (request, response, routeData) =>
            //        response.WriteAsync($"API Get {string.Join(", ", routeData.Values)}")
            //    )
            //          .MapMiddlewareRoute("api/middleware", (appBuilder) => appBuilder.Use((httpContext, next) => httpContext.Response.WriteAsync("Middleware!")))
            //    //      .MapRoute(
            //    //        name: "AllVerbs",
            //    //        template: "api/all/{name}/{lastName?}",
            //    //        defaults: new { lastName = "Doe" },
            //    //        constraints: new { lastName = new RegexRouteConstraint(new Regex("[a-zA-Z]{3}", RegexOptions.CultureInvariant, RegexMatchTimeout)) });
            //    ;
            //});
            app.UseMiddleware<PhyahMiddleware>();
        }
    }
}
