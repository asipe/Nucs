using Autofac;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Dependency.Modules {
  public class IOAbstractionsModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);
      builder
        .RegisterType<DotNetFile>()
        .SingleInstance()
        .As<IFile>();
    }
  }
}