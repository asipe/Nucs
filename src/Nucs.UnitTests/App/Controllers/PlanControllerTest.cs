using NUnit.Framework;
using Nucs.App.Controllers;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.Controllers {
  [TestFixture]
  public class PlanControllerTest : BaseTestCase {
    [Test]
    public void TestGetAllGivesEmptyArray() {
      Assert.That(new PlanController().GetAll(), Is.Empty);
    }
  }
}