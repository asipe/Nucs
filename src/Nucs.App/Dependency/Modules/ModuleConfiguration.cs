﻿using Autofac;
using MadCat.Core;

namespace Nucs.App.Dependency.Modules {
  public class ModuleConfiguration {
    public ModuleConfiguration(IConfig config) {
      mConfig = config;
    }

    public void Initialize(ContainerBuilder builder) {
      builder.RegisterModule(new MappingModule());
      builder.RegisterModule(new AbstractionsModule());
      builder.RegisterModule(new IndexControllerModule(mConfig));
      builder.RegisterModule(new CssControllerModule(mConfig));
      builder.RegisterModule(new JsControllerModule(mConfig));
      builder.RegisterModule(new RepositoryModule(mConfig));
      builder.RegisterModule(new PlanControllerModule());
    }

    private readonly IConfig mConfig;
  }
}