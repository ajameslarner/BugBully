using Autofac;
using Autofac.Integration.Mvc;
using BugBully.Data;
using BugBully.Models;
using BugBully.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Services.Description;

namespace BugBully
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Global configs
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Dependency Injection
            var builder = new ContainerBuilder();
            builder.RegisterType<BugBullyContext>().As<IRepository>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}