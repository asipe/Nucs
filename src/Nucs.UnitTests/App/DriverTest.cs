using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Nucs.Core.App;
using Nucs.Core.App.Controller;
using Nucs.Core.App.UI;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App {
  [TestFixture]
  public class DriverTest : BaseTestCase {
    [Test]
    public void TestDoesNotExecutePumpIfControllerTerminated() {
      mController.Setup(c => c.Terminated).Returns(true);
      mDriver.Start();
    }

    [Test]
    public void TestExecutesPumpUntilTerminated() {
      var terminatedStates = new Queue<bool>(BA(false, false, true));
      mController.Setup(c => c.Terminated).Returns(terminatedStates.Dequeue);
      mPump.Setup(p => p.Execute());
      mDriver.Start();
      mPump.Verify(p => p.Execute(), Times.Exactly(2));
    }

    [SetUp]
    public void DoSetup() {
      mController = Mok<IController>();
      mPump = Mok<IMessagePump>();
      mDriver = new Driver(mController.Object, mPump.Object);
    }

    private Mock<IController> mController;
    private Mock<IMessagePump> mPump;
    private Driver mDriver;
  }
}