using NUnit.Framework;
using Nucs.Core.Model.External;

namespace Nucs.UnitTests.Core.Model.External {
  [TestFixture]
  public class PlanDtoTest : NucsBaseTestCase {
    [Test]
    public void TestDefaults() {
      var plan = new PlanDto();
      Assert.That(plan.Executable, Is.Null);
      Assert.That(plan.Assembly, Is.Null);
      Assert.That(plan.Run, Is.Null);
      Assert.That(plan.ID, Is.Null);
    }
  }
}