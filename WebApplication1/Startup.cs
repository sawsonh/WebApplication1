using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Demo.Core.Constants;
using Demo.Core.Services;
using Demo.Infrastructure.DependencyResolution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Demo.UI.AspNetCore
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IContainer Container { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Define policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly",
                    policy => policy.RequireClaim("UserGroup", "Administrator", "Admin"));
            });
            
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Register Autofac DI
            var builder = new ContainerBuilder();
            AutofacResolution.RegisterTypes(builder, Configuration);
            builder.Populate(services);
            Container = builder.Build();
            return Container.Resolve<IServiceProvider>();
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

            app.UseErrorLogging();

            app.UseStaticFiles();

            var cfgSvc = Container.Resolve<IConfigService>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromHours(1),
                Events = new CookieAuthenticationEvents
                {
                    OnSigningIn = ctx =>
                    {
                        var identity = (ClaimsIdentity)ctx.Principal.Identity;
                        foreach (var claim in Container.Resolve<IUserService>().GetClaims(identity))
                        {
                            identity.AddClaim(claim);
                        }
                        return Task.FromResult(0);
                    }
                }
            });
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",
                ResponseType = OpenIdConnectResponseType.Code,
                Authority = cfgSvc.GetValue<string>(ConfigurationKeys.Authority),
                ClientId = cfgSvc.GetValue<string>(ConfigurationKeys.ClientId),
                ClientSecret = cfgSvc.GetValue<string>(ConfigurationKeys.ClientSecret),
                // Get all user info
                GetClaimsFromUserInfoEndpoint = true,
                // Explicitly define what user info you want
                //Scope = {"profile", "email", "address", "phone", "offline_access"},
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true
                },
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
