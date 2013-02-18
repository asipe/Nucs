using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using Nucs.App.Controllers;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.UnitTests.App.Controllers {
  [TestFixture]
  public class PlanControllerTest : NucsBaseTestCase {
    [Test]
    public void TestGetAllPlansWithNoPlansGivesEmtpy() {
      mRepo.Setup(r => r.List()).Returns(BA<Plan>());
      Assert.That(mController.GetAllPlans(), Is.Empty);
    }

    [Test]
    public void TetGetAllPlansWithPlans() {
      var plans = CM<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      Compare(mController.GetAllPlans(), plans);
    }

    [Test]
    public void TestGetSinglePlanByIDExists() {
      var plans = CM<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      Compare(mController.GetPlan(plans[0].ID), plans[0]);
      Compare(mController.GetPlan(plans[2].ID), plans[2]);
    }

    [Test]
    public void TestGetSinglePlanByIDWhichDoesNotExistThrows() {
      var plans = CM<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      Assert.Throws<InvalidOperationException>(() => mController.GetPlan(CA<string>()));
    }

    [Test]
    public void TestDeletePlan() {
      mRepo.Setup(r => r.Delete("abc"));
      mController.DeletePlan("abc");
    }

    [Test]
    public void TestCreatePlan() {
      var plan = CA<Plan>();
      var config = new HttpConfiguration();
      using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/plans")) {
        var routeData = new HttpRouteData(config.Routes.MapHttpRoute("defaultapi", "api/{controller}/{id}"), new HttpRouteValueDictionary {{"controller", "plans"}});
        mController.ControllerContext = new HttpControllerContext(config, routeData, request);
        mController.Request = request;
        mController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        mController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

        mRepo.Setup(r => r.Add(plan));
        using (var response = mController.PostPlan(plan)) {
          Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
          Assert.That(response.Headers.Location.AbsoluteUri, Is.EqualTo("http://localhost/api/plans/" + plan.ID));
          var result = response.Content.ReadAsStringAsync();
          result.Wait();
          var actual = JsonConvert.DeserializeObject<Plan>(result.Result);
          Compare(actual, plan);
        }
      }
    }

    [Test]
    public void TestPutPlan() {
      var plan = CA<Plan>();
      mRepo.Setup(r => r.Update(plan));
      mController.PutPlan(plan);
    }

    [SetUp]
    public void DoSetup() {
      mRepo = Mok<IPlanDetailRepository>();
      mController = new PlansController(mRepo.Object);
    }

    private PlansController mController;
    private Mock<IPlanDetailRepository> mRepo;
  }
}