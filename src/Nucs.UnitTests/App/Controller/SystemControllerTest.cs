using NUnit.Framework;
using Nucs.Core.App.Controller;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.Controller {
  [TestFixture]
  public class SystemControllerTest : BaseTestCase {
    [Test]
    public void TestDefaults() {
      Assert.That(mController.Terminated, Is.False);
    }

    [Test]
    public void TestTerminateSetsTerminatedFlag() {
      mController.Terminate();
      Assert.That(mController.Terminated, Is.True);
    }

    [SetUp]
    public void DoSetup() {
      mController = new SystemController();
    }

    private SystemController mController;
  }
}