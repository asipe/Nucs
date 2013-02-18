using NUnit.Framework;
using Nucs.Core.Model;
using Nucs.Core.Model.External;
using Nucs.Core.Serialization;

namespace Nucs.UnitTests.Serialization {
  [TestFixture]
  public class SerializerTest : NucsBaseTestCase {
    [Test]
    public void TestBasicSerialization() {
      var serializer = new Serializer();
      var expected = CA<Plan>();
      var json = serializer.Serialize(expected);
      Compare(serializer.Deserialize<Plan>(json), expected);
    }
  }
}