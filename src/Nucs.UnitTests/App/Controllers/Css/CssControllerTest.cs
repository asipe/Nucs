using Moq;
using NUnit.Framework;
using Nucs.App.Controllers.Css;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.Controllers.Css {
  [TestFixture]
  public class CssControllerTest : BaseTestCase {
    [Test]
    public void TestExecute() {
      mFile.Setup(f => f.ReadAllText(@"css\nucs.css")).Returns("css");
      var resp = mController.Get();
      Assert.That(resp.Content.Headers.ContentType.MediaType, Is.EqualTo("text/css"));
      var task = resp.Content.ReadAsStringAsync();
      task.Wait();
      Assert.That(task.Result, Is.EqualTo("css"));
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mController = new CssController(mFile.Object);
    }

    private Mock<IFile> mFile;
    private CssController mController;
  }
}