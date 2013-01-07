using NUnit.Framework;
using Nucs.App.Controllers;

namespace Nucs.UnitTests.App.Controllers {
  [TestFixture]
  public class PlanControllerTest : NucsBaseTestCase {
    [Test]
    public void TestGetAllGivesEmptyArray() {
      Assert.That(new PlanController().GetAll(), Is.Empty);
    }
  }
}