using Autofac;
using MadCat.Core;
using Nucs.App.Controllers.Css;

namespace Nucs.App.Dependency.Modules {
  public class CssControllerModule : Module {
    public CssControllerModule(IConfig config) {
      mConfig = config;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<CssController>()
        .InstancePerLifetimeScope()
        .WithParameter("assetDir", mConfig.Get<string>("nucs-asset-dir"));
    }

    private readonly IConfig mConfig;
  }
}