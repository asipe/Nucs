using Moq;
using NUnit.Framework;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;
using Nucs.Core.Serialization;
using Nucs.Core.Storage;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Core.OID;

namespace Nucs.UnitTests.Core.Storage {
  [TestFixture]
  public class PlanSpecRepositoryTest : NucsBaseTestCase {
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
      mOIDProvider.Setup(p => p.GetID()).Returns("ABC");
      mSerializer.Setup(s => s.Serialize(plan)).Returns(json);
      mFile.Setup(f => f.WriteAllText(@"c:\data\ABC.json", json));
      mRepo.Add(plan);
      Assert.That(plan.ID, Is.EqualTo("ABC"));
    }

    [Test]
    public void TestDeletePlan() {
      var plan = CA<PlanDto>();
      mFile.Setup(f => f.Delete(_Path + plan.ID + ".json"));
      mRepo.Delete(plan.ID);
    }

    [Test]
    public void TestUpdatePlan() {
      var plan = CA<Plan>();
      var json = CA<string>();
      plan.ID = "ABC";
      mSerializer.Setup(s => s.Serialize(plan)).Returns(json);
      mFile.Setup(f => f.WriteAllText(@"c:\data\ABC.json", json));
      mRepo.Update(plan);
    }

    [SetUp]
    public void DoSetup() {
      mFile = Mok<IFile>();
      mDirectory = Mok<IDirectory>();
      mSerializer = Mok<ISerializer>();
      mOIDProvider = Mok<IOIDProvider>();
      mRepo = new PlanSpecRepository(_Path, mFile.Object, mDirectory.Object, mSerializer.Object, mOIDProvider.Object);
    }

    private const string _Path = @"c:\data\";
    private Mock<IFile> mFile;
    private Mock<ISerializer> mSerializer;
    private PlanSpecRepository mRepo;
    private Mock<IDirectory> mDirectory;
    private Mock<IOIDProvider> mOIDProvider;
  }
}