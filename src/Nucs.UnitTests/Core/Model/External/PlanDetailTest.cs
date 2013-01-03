using NUnit.Framework;
using Nucs.Core.Model.External;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Core.Model.External {
  [TestFixture]
  public class PlanDetailTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var detail = new PlanDetail();
      Assert.That(detail.Executable, Is.Null);
      Assert.That(detail.Assembly, Is.Null);
      Assert.That(detail.Run, Is.Null);
    }
  }
}