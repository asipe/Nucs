using Moq;
using NUnit.Framework;
using Nucs.App.Controllers.Js;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.Controllers.Js {
  [TestFixture]
  public class JsControllerTest : BaseTestCase {
    [Test]
    public void TestExecute() {
      mFile.Setup(f => f.ReadAllText(@"scripts\nucs.js")).Returns("js");
      var resp = mController.Get();
      Assert.That(resp.Content.Headers.ContentType.MediaType, Is.EqualTo("text/javascript"));
      var task = resp.Content.ReadAsStringAsync();
      task.Wait();
      Assert.That(task.Result, Is.EqualTo("js"));
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mController = new JsController(mFile.Object);
    }

    private Mock<IFile> mFile;
    private JsController mController;
  }
}