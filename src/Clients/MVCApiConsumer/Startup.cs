using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace MVCClientConsumesAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            // The combination of the following configs...
            // UseCookieAuthentication,
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(),
            //app.UseOpenIdConnectAuthentication 
            // ... will cause 
            // Browser to redirect to ID4 server on navigate to protected controller.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"

            });

            // The OpenID Connect middleware configuration needs pointer to your IdentityServer, a client ID and tell it which middleware will do the local signin (namely the cookies middleware). As well, turned off the JWT claim type mapping to allow well-known claims (e.g. ‘sub’ and ‘idp’) to flow through unmolested:
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,

                ClientId = "MVCApiConsumer",
                ClientSecret = "MVCApiConsumer.Secret",

                // The following settings mean “use hybrid flow”.
                ResponseType = "code id_token",
                Scope = { "MyThings", "offline_access" },
                // offline_access will return Refresh Token and requires a matching setting in client def in ID4
                // Here are some other Scope Examples:
                //    Scope = “openid email profile read write offline_access”
                //       api access (read and write) 
                GetClaimsFromUserInfoEndpoint = true, // tells the middleware to go to the user info endpoint to retrieve additional claims after getting an identity token.

                SaveTokens = true
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
