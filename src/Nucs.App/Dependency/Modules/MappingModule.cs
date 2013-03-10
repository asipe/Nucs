using Autofac;
using Nucs.Core.Mapping;

namespace Nucs.App.Dependency.Modules {
  public class MappingModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      new MapperConfiguration().Configure();

      builder
        .RegisterType<ObjectMapper>()
        .SingleInstance()
        .As<IObjectMapper>();
    }
  }
}