using NUnit.Framework;
using SupaCharge.Testing;

namespace Nucs.UnitTests {
  [TestFixture]
  public class SampleTest : BaseTestCase {
    [Test]
    public void TestHello() {
      Assert.That(true, Is.True);
    }
  }
}