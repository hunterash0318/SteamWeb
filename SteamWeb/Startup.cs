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
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));

            // NOTE: View Components are a new thing in ASP.NET Core MVC, I'm not sure how I should be using them yet.
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            //services.AddSingleton<IAuthorizationHandler>(new AuthorizationHandlerToSecurityValidatorAdapter(container));

            // This extensiom method makes sure all of our ASP.NET web requests are wrapped in an Async Scoped lifestyle
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            var SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(Configuration["steamdb"]))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Startup>())
                .BuildSessionFactory();

            container.RegisterSingleton(typeof(ISessionFactory), SessionFactory);

            // Register how to create an ISession using an ISessionFactory.
            container.Register(() => container.GetService<ISessionFactory>().OpenSession(), Lifestyle.Scoped);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Games}/{action=Index}/{id?}");
            });


        }
    }


}
