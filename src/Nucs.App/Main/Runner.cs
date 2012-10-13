using System;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Autofac;
using Nucs.App.Dependency;
using Nucs.App.Dependency.Modules;

namespace Nucs.App.Main {
  public class Runner {
    public void Start() {
      try {
        DoStart();
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
    }

    private static void DoStart() {
      var config = new HttpSelfHostConfiguration("http://localhost:49991");
      ConfigureRoutes(config);
      ConfigureDependencyResolver(config);
      using (var server = new HttpSelfHostServer(config)) {
        server.OpenAsync().Wait();
        Console.WriteLine("Press Enter to quit.");
        Console.ReadLine();
        server.CloseAsync().Wait();
      }
    }

    private static void ConfigureRoutes(HttpConfiguration config) {
      config.Routes.MapHttpRoute("default", "index.html", new {controller = "Index"});
    }

    private static void ConfigureDependencyResolver(HttpConfiguration config) {
      var builder = new ContainerBuilder();
      new ModuleConfiguration().Initialize(builder);
      config.DependencyResolver = new DependencyResolver(builder.Build());
    }
  }
}