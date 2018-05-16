using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SteamWeb.Infrastructure.Authentication;
using SteamWeb.Infrastructure;
using System.Security.Claims;
using AutoMapper;

namespace SteamWeb
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // New code for adding session state (cookies)
            /*
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });
            */
            // End session state code

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));

            // New cookie stuff
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // NOTE: View Components are a new thing in ASP.NET Core MVC, I'm not sure how I should be using them yet.
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            //services.AddSingleton<IAuthorizationHandler>(new AuthorizationHandlerToSecurityValidatorAdapter(container));

            // This extensiom method makes sure all of our ASP.NET web requests are wrapped in an Async Scoped lifestyle
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // New code
            //app.UseSession();
            // End new code

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            container.RegisterSingleton(typeof(IAuthenticationManager), new AspNetCookieAuthenticationAdapter(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>()));

            container.RegisterSingleton(typeof(ICurrentUserContext), new AspNetCurrentUserContext(GetHttpContextProvider(app)));

            var SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(Configuration["steamdb"]))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Startup>())
                .BuildSessionFactory();

            container.RegisterSingleton(typeof(ISessionFactory), SessionFactory);

            // Register how to create an ISession using an ISessionFactory.
            container.Register(() => container.GetService<ISessionFactory>().OpenSession(), Lifestyle.Scoped);

            Mapper.Initialize(cfg => cfg.AddProfiles(typeof(Startup).Assembly));

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });



        }

        private Func<ClaimsPrincipal> GetHttpContextProvider(IApplicationBuilder app)
        {
            var accessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            return () =>
            {
                if (accessor.HttpContext == null)
                {
                    throw new InvalidOperationException("No HttpContext");
                }
                return accessor.HttpContext.User;
            };
        }

    }


}
