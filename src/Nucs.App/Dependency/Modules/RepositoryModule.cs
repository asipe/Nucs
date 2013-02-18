using Autofac;
using MadCat.Core;
using Nucs.Core.Storage;

namespace Nucs.App.Dependency.Modules {
  public class RepositoryModule : Module {
    public RepositoryModule(IConfig config) {
      mConfig = config;
    }

    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<PlanDetailRepository>()
        .InstancePerLifetimeScope()
        .As<IPlanDetailRepository>()
        .WithParameter("storePath", mConfig.Get<string>("nucs-config-dir"));
    }

    private readonly IConfig mConfig;
  }
}