using Autofac;
using MadCat.Core;
using Nucs.Core.Storage;

namespace Nucs.App.Dependency.Modules {
  public class StoreModule : Module {
    public StoreModule(IConfig config) {
      mConfig = config;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<PlanStore>()
        .InstancePerLifetimeScope()
        .As<IPlanStore>()
        .WithParameter("storePath", mConfig.Get<string>("nucs-config-dir"));
    }

    private readonly IConfig mConfig;
  }
}