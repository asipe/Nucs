using System;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Autofac;
using MadCat.Core;
using Nucs.App.Dependency;
using Nucs.App.Dependency.Modules;

namespace Nucs.App.Main {
  public class Runner {
    public void Start(IConfig appConfig) {
      try {
        DoStart(appConfig);
      } catch (Exception e) {
        Console.WriteLine(e);
      }
    }

    private static void DoStart(IConfig appConfig) {
      var httpConfig = new HttpSelfHostConfiguration("http://localhost:49991");
      ConfigureRoutes(httpConfig);
      ConfigureDependencyResolver(httpConfig, appConfig);
      using (var server = new HttpSelfHostServer(httpConfig)) {
        server.OpenAsync().Wait();
        Console.WriteLine("Press Enter to quit.");
        Console.ReadLine();
        server.CloseAsync().Wait();
      }
    }

    private static void ConfigureRoutes(HttpConfiguration httpConfig) {
      httpConfig.Routes.MapHttpRoute("defaultapi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
      httpConfig.Routes.MapHttpRoute("default", "index.html", new {controller = "Index"});
      httpConfig.Routes.MapHttpRoute("css", "css/nucs.css", new {controller = "Css"});
      httpConfig.Routes.MapHttpRoute("js", "scripts/nucs.js", new {controller = "Js"});
    }

    private static void ConfigureDependencyResolver(HttpConfiguration httpConfig, IConfig appConfig) {
      var builder = new ContainerBuilder();
      new ModuleConfiguration(appConfig).Initialize(builder);
      httpConfig.DependencyResolver = new DependencyResolver(builder.Build());
    }
  }
}