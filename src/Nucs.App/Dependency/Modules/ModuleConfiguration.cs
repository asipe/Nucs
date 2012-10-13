using Autofac;

namespace Nucs.App.Dependency.Modules {
  public class ModuleConfiguration {
    public void Initialize(ContainerBuilder builder) {
      builder.RegisterModule(new IOAbstractionsModule());
      builder.RegisterModule(new IndexControllerModule());
    }
  }
}