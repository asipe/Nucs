using System;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Nucs.App.Dependency;

namespace Nucs.UnitTests.App.Dependency {
  [TestFixture]
  public class DependencyResolverTest : NucsBaseTestCase {
    private class StubDisposable : IDisposable {
      public bool Disposed{get;private set;}

      public void Dispose() {
        Disposed = true;
      }
    }

    [Test]
    public void TestGetServiceResolvesFromContainer() {
      Assert.That(mResolver.GetService(typeof(int)), Is.EqualTo(3));
    }

    [Test]
    public void TestGetServiceReturnsNullIfRegistrationNotFound() {
      Assert.That(mResolver.GetService(typeof(DateTime)), Is.Null);
    }

    [Test]
    public void TestGetServicesDoesNotWorkForIntrinsics() {
      Assert.Throws<InvalidCastException>(() => mResolver.GetServices(typeof(int)));
    }

    [Test]
    public void TestGetServicesResolvesFromContainer() {
      Assert.That(mResolver.GetServices(typeof(string)).Cast<string>(), Is.EquivalentTo(BA("hello", "good bye")));
    }

    [Test]
    public void TestGetServicesReturnsEmptyIfRegistrationNotFound() {
      Assert.That(mResolver.GetServices(typeof(DateTime)), Is.Empty);
    }

    [Test]
    public void TestBeginScopeStartsLifetimeScope() {
      StubDisposable tmp;
      using (var scope = mResolver.BeginScope())
        tmp = (StubDisposable)scope.GetService(typeof(StubDisposable));
      Assert.That(tmp.Disposed, Is.True);
    }

    [Test]
    public void TestBeginScopeReturnsDifferentPerLifetimeScope() {
      using (var s1 = mResolver.BeginScope())
      using (var s2 = mResolver.BeginScope()) {
        Assert.That(s1, Is.Not.SameAs(s2));
        Assert.That(s1.GetService(typeof(StubDisposable)), Is.Not.SameAs(s2.GetService(typeof(StubDisposable))));
      }
    }

    [SetUp]
    public void DoSetup() {
      var builder = new ContainerBuilder();
      builder.Register(c => 2);
      builder.Register(c => 3);
      builder.Register(c => "hello");
      builder.Register(c => "good bye");
      builder.RegisterType<StubDisposable>().InstancePerLifetimeScope();
      mContainer = builder.Build();
      mResolver = new DependencyResolver(mContainer);
    }

    [TearDown]
    public void DoTearDown() {
      if (mContainer != null)
        mContainer.Dispose();
    }

    private DependencyResolver mResolver;
    private IContainer mContainer;
  }
}