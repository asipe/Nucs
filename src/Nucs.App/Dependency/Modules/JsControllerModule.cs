using Autofac;
using MadCat.Core;
using Nucs.App.Controllers.Js;

namespace Nucs.App.Dependency.Modules {
  public class JsControllerModule : Module {
    public JsControllerModule(IConfig config) {
      mConfig = config;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<JsController>()
        .InstancePerLifetimeScope()
        .WithParameter("assetDir", mConfig.Get<string>("nucs-asset-dir"));
    }

    private readonly IConfig mConfig;
  }
}