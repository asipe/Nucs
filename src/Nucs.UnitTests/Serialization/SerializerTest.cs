using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Nucs.Core.Model;
using Nucs.Core.Serialization;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Serialization {
  [TestFixture]
  public class SerializerTest : BaseTestCase {
    [Test]
    public void TestBasicSerialization() {
      var original = CA<Plan>();
      var json = new Serializer().Serialize(original);
      var current = new Serializer().Deserialize<Plan>(json);
      var compare = new CompareObjects();
      Assert.That(compare.Compare(original, current), Is.True, compare.DifferencesString);
    }
  }
}