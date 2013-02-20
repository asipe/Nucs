using NUnit.Framework;
using Nucs.Core.Model.Internal;

namespace Nucs.UnitTests.Core.Model.Internal {
  [TestFixture]
  public class PlanSpecTest : NucsBaseTestCase {
    [Test]
    public void TestDefaults() {
      var plan = new PlanSpec();
      Assert.That(plan.Executable, Is.Null);
      Assert.That(plan.Assembly, Is.Null);
      Assert.That(plan.Run, Is.Null);
      Assert.That(plan.ID, Is.Null);
    }
  }
}