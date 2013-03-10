using AutoMapper;
using NUnit.Framework;
using Nucs.Core.Mapping;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Core.Mapping {
  [TestFixture]
  public class MapperConfigurationTest : BaseTestCase {
    [Test]
    public void TestMappingConfiguration() {
      Mapper.AssertConfigurationIsValid();
    }

    [SetUp]
    public void DoSetup() {
      new MapperConfiguration().Configure();
    }

    [TearDown]
    public void DoTearDown() {
      Mapper.Reset();
    }
  }
}