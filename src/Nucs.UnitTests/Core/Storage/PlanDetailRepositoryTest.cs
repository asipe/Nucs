using Moq;
using NUnit.Framework;
using Nucs.Core.Model.External;
using Nucs.Core.Serialization;
using Nucs.Core.Storage;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.UnitTests.Core.Storage {
  [TestFixture]
  public class PlanDetailRepositoryTest : NucsBaseTestCase {
    [Test]
    public void TestListWhenEmpty() {
      var files = BA<string>();
      mDirectory.Setup(d => d.GetFiles(_Path, "*.json")).Returns(files);
      Assert.That(mRepo.List(), Is.Empty);
    }

    [Test]
    public void TestListSinglePlan() {
      var files = CM<string>(1);
      var jsons = CM<string>(1);
      var plans = CM<Plan>(1);
      mDirectory.Setup(d => d.GetFiles(_Path, "*.json")).Returns(files);
      mFile.Setup(f => f.ReadAllText(_Path + files[0])).Returns(jsons[0]);
      mSerializer.Setup(s => s.Deserialize<Plan>(jsons[0])).Returns(plans[0]);
      Assert.That(mRepo.List(), Is.EqualTo(plans));
    }

    [Test]
    public void TestListMultiplePlans() {
      var files = CM<string>();
      var jsons = CM<string>();
      var plans = CM<Plan>();
      mDirectory.Setup(d => d.GetFiles(_Path, "*.json")).Returns(files);
      for (var x = 0; x < 3; x++) {
        var x1 = x;
        mFile.Setup(f => f.ReadAllText(_Path + files[x1])).Returns(jsons[x1]);
        mSerializer.Setup(s => s.Deserialize<Plan>(jsons[x1])).Returns(plans[x1]);
      }
      Assert.That(mRepo.List(), Is.EqualTo(plans));
    }

    [Test]
    public void TestAddPlan() {
      var plan = CA<Plan>();
      var json = CA<string>();
      mSerializer.Setup(s => s.Serialize(plan)).Returns(json);
      mFile.Setup(f => f.WriteAllText(_Path + plan.ID + ".json", json));
      mRepo.Add(plan);
    }

    [Test]
    public void TestDeletePlan() {
      var plan = CA<Plan>();
      mFile.Setup(f => f.Delete(_Path + plan.ID + ".json"));
      mRepo.Delete(plan.ID);
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mDirectory = Mok<IDirectory>();
      mSerializer = Mok<ISerializer>();
      mRepo = new PlanDetailRepository(_Path, mFile.Object, mDirectory.Object, mSerializer.Object);
    }

    private const string _Path = @"c:\data\";
    private Mock<IFile> mFile;
    private Mock<ISerializer> mSerializer;
    private PlanDetailRepository mRepo;
    private Mock<IDirectory> mDirectory;
  }
}