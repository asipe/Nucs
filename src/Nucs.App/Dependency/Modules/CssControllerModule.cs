using Autofac;
using Nucs.App.Controllers.Css;

namespace Nucs.App.Dependency.Modules {
  public class CssControllerModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<CssController>()
        .InstancePerLifetimeScope();
    }
  }
}