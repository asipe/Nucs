using Moq;
using NUnit.Framework;
using Nucs.Core.App.Command;
using Nucs.Core.App.UI;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.UI {
  [TestFixture]
  public class MessagePumpTest : BaseTestCase {
    [Test]
    public void TestExecute() {
      mUI.Setup(ui => ui.Write("nucs>"));
      mUI.Setup(ui => ui.ReadLine()).Returns("cmd args");
      mHandler.Setup(h => h.Handle("cmd args"));
      mPump.Execute();
    }

    [SetUp]
    public void DoSetup() {
      mUI = Mok<IUserInterface>();
      mHandler = Mok<IHandler>();
      mPump = new MessagePump(mUI.Object, mHandler.Object);
    }

    private Mock<IUserInterface> mUI;
    private Mock<IHandler> mHandler;
    private MessagePump mPump;
  }
}