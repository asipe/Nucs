using Autofac;
using Nucs.App.Controllers.Index;

namespace Nucs.App.Dependency.Modules {
  public class IndexControllerModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<IndexController>()
        .InstancePerLifetimeScope();
    }
  }
}