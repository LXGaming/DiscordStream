using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchLib.Webhook.Extensions;

namespace LXGaming.DiscordStream.Server {

    public class Startup {

        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvcCore()
                .AddTwitchWebHooks();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment) {
            if (environment.IsDevelopment()) {
                application.UseDeveloperExceptionPage();
            } else {
                application.UseHsts();
                application.UseStatusCodePagesWithReExecute("/error/{0}");
            }

            application.UseRouting();
            application.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}