using Autofac;
using Nucs.Core.Serialization;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Dependency.Modules {
  public class AbstractionsModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      builder
        .RegisterType<DotNetFile>()
        .SingleInstance()
        .As<IFile>();

      builder
        .RegisterType<DotNetDirectory>()
        .SingleInstance()
        .As<IDirectory>();

      builder
        .RegisterType<Serializer>()
        .SingleInstance()
        .As<ISerializer>();
    }
  }
}