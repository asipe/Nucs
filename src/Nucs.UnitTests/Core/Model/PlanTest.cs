using NUnit.Framework;
using Nucs.Core.Model;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Core.Model {
  [TestFixture]
  public class PlanTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      var plan = new Plan();
      Assert.That(plan.Executable, Is.Null);
      Assert.That(plan.Assembly, Is.Null);
      Assert.That(plan.Run, Is.Null);
      Assert.That(plan.ID, Is.Null);
    }
  }
}