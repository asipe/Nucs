using Autofac;
using MadCat.Core;

namespace Nucs.App.Dependency.Modules {
  public class ModuleConfiguration {
    public ModuleConfiguration(IConfig config) {
      mConfig = config;
    }

    public void Initialize(ContainerBuilder builder) {
      builder.RegisterModule(new IOAbstractionsModule());
      builder.RegisterModule(new IndexControllerModule());
      builder.RegisterModule(new CssControllerModule());
      builder.RegisterModule(new JsControllerModule());
    }

    private readonly IConfig mConfig;
  }
}