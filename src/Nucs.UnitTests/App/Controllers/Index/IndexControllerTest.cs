using Moq;
using NUnit.Framework;
using Nucs.App.Controllers.Index;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.Controllers.Index {
  [TestFixture]
  public class IndexControllerTest : BaseTestCase {
    [Test]
    public void TestExecute() {
      mFile.Setup(f => f.ReadAllText(@"views\index\index.html")).Returns("html");
      var resp = mController.Get();
      Assert.That(resp.Content.Headers.ContentType.MediaType, Is.EqualTo("text/html"));
      var task = resp.Content.ReadAsStringAsync();
      task.Wait();
      Assert.That(task.Result, Is.EqualTo("html"));
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mController = new IndexController(mFile.Object);
    }

    private Mock<IFile> mFile;
    private IndexController mController;
  }
}