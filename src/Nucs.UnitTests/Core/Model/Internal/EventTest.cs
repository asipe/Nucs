using NUnit.Framework;
using Nucs.Core.Model.Internal;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Core.Model.Internal {
  [TestFixture]
  public class EventTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var evt = new Event("abc");
      Assert.That(evt.Name, Is.EqualTo("abc"));
    }
  }
}