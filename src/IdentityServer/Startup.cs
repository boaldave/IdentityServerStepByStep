using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // LOH 08/09/17 - AddIdentityServer registers the IdentityServer services in DI. It also registers an in-memory store for runtime state. This is useful for development scenarios. For production scenarios you need a persistent or shared store like a database or cache for that. See the EntityFramework quickstart for more information.

            // Configure identity server with in-memory stores, keys, clients and resources
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(LogLevel.Debug);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.AddConsole();
            }

            // LOH 08/09/17 - AddTemporarySigningCredential extension creates temporary key material for signing tokens on every start.Again this might be useful to get started, but needs to be replaced by some persistent key material for production scenarios. See the cryptography docs for more information.
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
