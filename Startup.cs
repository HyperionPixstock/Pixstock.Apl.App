using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Hyperion.Pf.Workflow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pixstock.apl.app.core;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace pixstock.apl.app
{
    public class Startup
    {
        private Container mContainer = new Container();

        private ContentMainWorkflowEventEmiter emiter;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();
            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeContainer(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //this.emiter = new ContentMainWorkflowEventEmiter();
            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }
        }

        public async void ElectronBootstrap()
        {
            Console.WriteLine("Execute CreateWindowAsync");
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1400,
                Height = 900,
                WebPreferences = new WebPreferences
                {
                    WebSecurity = false
                },
                Show = false
            });

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Pixstock Client");
            //this.emiter.Initialize();
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            mContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHostedService, QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(mContainer));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(mContainer));

            services.EnableSimpleInjectorCrossWiring(mContainer);
            services.UseSimpleInjectorAspNetRequestScoping(mContainer);
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            mContainer.RegisterMvcControllers(app);
            mContainer.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            //container.Register<IUserService, UserService>(Lifestyle.Scoped);

            // Cross-wire ASP.NET services (if any). For instance:
            mContainer.CrossWire<ILoggerFactory>(app);

            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/

            var queue = app.ApplicationServices.GetService<IBackgroundTaskQueue>(); // ASPNETに登録したサービスのインスタンスを取得する
            mContainer.RegisterInstance<IBackgroundTaskQueue>(queue); // サービスオブジェクトを、他のオブジェクトにインジェクションするためにDIに登録する

            var memCache = app.ApplicationServices.GetService<IMemoryCache>(); // ASPNETに登録したサービスのインスタンスを取得する
            mContainer.RegisterInstance<IMemoryCache>(memCache);

            // Ipcマネージャの初期化
            var ipcBridge = new IpcBridge(mContainer);
            mContainer.RegisterInstance<IRequestHandlerFactory>(ipcBridge.Initialize());

            // ServiceDistorionマネージャの初期化
            var serviceDistoributionManager = new ServiceDistoributionManager(mContainer);
            serviceDistoributionManager.Initialize();
            mContainer.RegisterInstance<IServiceDistoributor>(serviceDistoributionManager);

            // Intentマネージャの初期化
            mContainer.RegisterSingleton<IIntentManager, IntentManager>();

            // Screenマネージャの初期化
            mContainer.RegisterSingleton<IScreenManager, ScreenManager>();

            mContainer.Verify();
            serviceDistoributionManager.Verify();
        }
    }
}
