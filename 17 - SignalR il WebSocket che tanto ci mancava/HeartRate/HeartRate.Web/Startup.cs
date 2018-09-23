using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HeartRate.Web.HostedServices;
using HeartRate.Web.Hubs;
using HeartRate.Web.HubServices;

namespace HeartRate.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // Registro SignalR Hub
            services.AddSingleton<IHeartRateHubServices, HeartRateHubServices>();

            services.AddSignalR(t => t.EnableDetailedErrors = true);
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Registro Hosted Service per scodare gli eventi
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HeartRateHostedService>(x => 
                new HeartRateHostedService(Configuration.GetValue<string>("IoTHub"),
                                             Configuration.GetValue<string>("StorageConnectionString"),
                                             x.GetRequiredService<ILogger<HeartRateHostedService>>(),
                                             x.GetRequiredService<IHeartRateHubServices>()));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(routes =>
            {
                routes.MapHub<HeartRateHub>("/heartratehub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
