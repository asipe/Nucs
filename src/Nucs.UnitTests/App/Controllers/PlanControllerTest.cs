using System.Net;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using Nucs.App.Controllers;
using Nucs.Core.Model;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.UnitTests.App.Controllers {
  [TestFixture]
  public class PlanControllerTest : NucsBaseTestCase {
    [Test]
    public void TestGetAllEmptyGivesEmtpy() {
      mStore.Setup(s => s.List()).Returns(BA<Plan>());
      Assert.That(mController.GetPlans(), Is.Empty);
    }

    [Test]
    public void TesGetAllSinglePlanGivesSinglePlanDetail() {
      var plans = CM<Plan>(1);
      mStore.Setup(s => s.List()).Returns(plans);
      var expected = BA(new PlanDetail {
                                         Assembly = plans[0].Assembly,
                                         Executable = plans[0].Executable,
                                         Run = plans[0].Run
                                       });
      Compare(mController.GetPlans(), expected);
    }

    [Test]
    public void TesGetMultiplePlanGivesSinglePlanDetail() {
      var plans = CM<Plan>();
      mStore.Setup(s => s.List()).Returns(plans);
      var expected = BA(new PlanDetail {
                                         Assembly = plans[0].Assembly,
                                         Executable = plans[0].Executable,
                                         Run = plans[0].Run
                                       },
                        new PlanDetail {
                                         Assembly = plans[1].Assembly,
                                         Executable = plans[1].Executable,
                                         Run = plans[1].Run
                                       },
                        new PlanDetail {
                                         Assembly = plans[2].Assembly,
                                         Executable = plans[2].Executable,
                                         Run = plans[2].Run
                                       });
      Compare(mController.GetPlans(), expected);
    }

    [Test]
    public void TestDelete() {
      var id = CA<string>();
      mStore.Setup(s => s.Delete(id));
      Compare(new HttpResponseMessage {StatusCode = HttpStatusCode.Accepted}, mController.DeletePlan(id));
    }

    [Test]
    public void TestAdd() {
      var detail = CA<PlanDetail>();
      Compare(new HttpResponseMessage {StatusCode = HttpStatusCode.Created}, mController.CreatePlan(detail));
    }

    [SetUp]
    public void DoSetup() {
      mStore = Mok<IPlanStore>();
      mController = new PlanController(mStore.Object);
    }

    private Mock<IPlanStore> mStore;
    private PlanController mController;
  }
}