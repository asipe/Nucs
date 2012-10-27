using Autofac;
using MadCat.Core;
using Nucs.App.Controllers.Index;

namespace Nucs.App.Dependency.Modules {
  public class IndexControllerModule : Module {
    public IndexControllerModule(IConfig config) {
      mConfig = config;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<IndexController>()
        .InstancePerLifetimeScope()
        .WithParameter("assetDir", mConfig.Get<string>("nucs-asset-dir"));
    }

    private readonly IConfig mConfig;
  }
}