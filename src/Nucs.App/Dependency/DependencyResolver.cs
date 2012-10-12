using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Autofac;

namespace Nucs.App.Dependency {
  public class DependencyResolver : IDependencyResolver {
    private sealed class Scope : IDependencyScope {
      public Scope(ILifetimeScope scope) {
        mScope = scope;
      }

      public void Dispose() {
        mScope.Dispose();
      }

      public object GetService(Type serviceType) {
        return DependencyResolver.GetService(mScope, serviceType);
      }

      public IEnumerable<object> GetServices(Type serviceType) {
        return DependencyResolver.GetServices(mScope, serviceType);
      }

      private readonly ILifetimeScope mScope;
    }

    public DependencyResolver(IContainer container) {
      mContainer = container;
    }

    public void Dispose() {
      mContainer.Dispose();
    }

    public object GetService(Type serviceType) {
      return GetService(mContainer, serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType) {
      return GetServices(mContainer, serviceType);
    }

    public IDependencyScope BeginScope() {
      return new Scope(mContainer.BeginLifetimeScope());
    }

    private static object GetService(IComponentContext context, Type serviceType) {
      object service;
      context.TryResolve(serviceType, out service);
      return service;
    }

    private static IEnumerable<object> GetServices(IComponentContext context, Type serviceType) {
      return !context.IsRegistered(serviceType)
               ? Enumerable.Empty<object>()
               : GetServiceList(context, serviceType);
    }

    private static IEnumerable<object> GetServiceList(IComponentContext context, Type serviceType) {
      return (IEnumerable<object>)context.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType));
    }

    private readonly IContainer mContainer;
  }
}