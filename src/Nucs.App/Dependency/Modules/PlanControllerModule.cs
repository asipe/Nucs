using Autofac;
using Nucs.App.Controllers;

namespace Nucs.App.Dependency.Modules {
  public class PlanControllerModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<PlansController>()
        .InstancePerLifetimeScope();
    }
  }
}