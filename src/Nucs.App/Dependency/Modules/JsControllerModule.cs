using Autofac;
using Nucs.App.Controllers.Js;

namespace Nucs.App.Dependency.Modules {
  public class JsControllerModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<JsController>()
        .InstancePerLifetimeScope();
    }
  }
}