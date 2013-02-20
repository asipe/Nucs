using System;
using System.Linq;
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
using Nucs.Core.Mapping;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;
using Nucs.Core.Storage;

namespace Nucs.UnitTests.App.Controllers {
  [TestFixture]
  public class PlanControllerTest : NucsBaseTestCase {
    [Test]
    public void TestGetAllPlansWithNoPlansGivesEmtpy() {
      mRepo.Setup(r => r.List()).Returns(BA<PlanSpec>());
      Assert.That(mController.GetAllPlans(), Is.Empty);
    }

    [Test]
    public void TetGetAllPlansWithPlans() {
      var plans = CM<Plan>();
      var specs = PlanMapper.ToSpec(plans).ToArray();
      mRepo.Setup(r => r.List()).Returns(specs);
      Compare(mController.GetAllPlans().ToArray(), plans);
    }

    [Test]
    public void TestGetSinglePlanByIDExists() {
      var plans = CM<Plan>();
      var specs = PlanMapper.ToSpec(plans).ToArray();
      mRepo.Setup(r => r.List()).Returns(specs);
      Compare(mController.GetPlan(plans[0].ID), plans[0]);
      Compare(mController.GetPlan(plans[2].ID), plans[2]);
    }

    [Test]
    public void TestGetSinglePlanByIDWhichDoesNotExistThrows() {
      var plans = CM<Plan>();
      var specs = PlanMapper.ToSpec(plans).ToArray();
      mRepo.Setup(r => r.List()).Returns(specs);
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
      var spec = PlanMapper.ToSpec(plan);
      var config = new HttpConfiguration();
      using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/plans")) {
        var routeData = new HttpRouteData(config.Routes.MapHttpRoute("defaultapi", "api/{controller}/{id}"), new HttpRouteValueDictionary {{"controller", "plans"}});
        mController.ControllerContext = new HttpControllerContext(config, routeData, request);
        mController.Request = request;
        mController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        mController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

        mRepo.Setup(r => r.Add(It.Is<PlanSpec>(s => IsEqual(s, spec))));
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
      var spec = PlanMapper.ToSpec(plan);
      mRepo.Setup(r => r.Update(It.Is<PlanSpec>(s => IsEqual(s, spec))));
      mController.PutPlan(plan);
    }

    [SetUp]
    public void DoSetup() {
      mRepo = Mok<IPlanSpecRepository>();
      mController = new PlanController(mRepo.Object);
    }

    private PlanController mController;
    private Mock<IPlanSpecRepository> mRepo;
  }
}