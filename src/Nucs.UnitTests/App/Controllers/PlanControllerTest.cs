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
      var plans = BA<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      mMapper.Setup(m => m.Map<PlanDto[]>(plans)).Returns(BA<PlanDto>());
      Assert.That(mController.GetAllPlans(), Is.Empty);
    }

    [Test]
    public void TetGetAllPlansWithPlans() {
      var plandtos = CM<PlanDto>();
      var plans = CM<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      mMapper.Setup(m => m.Map<PlanDto[]>(plans)).Returns(plandtos);
      Compare(mController.GetAllPlans().ToArray(), plandtos);
    }

    [Test]
    public void TestGetSinglePlanByIDExists() {
      var plandtos = CM<PlanDto>();
      var plans = CM<Plan>();
      mRepo.Setup(r => r.List()).Returns(plans);
      mMapper.Setup(m => m.Map<PlanDto>(plans[0])).Returns(plandtos[0]);
      Compare(mController.GetPlan(plans[0].ID), plandtos[0]);
      mMapper.Setup(m => m.Map<PlanDto>(plans[2])).Returns(plandtos[2]);
      Compare(mController.GetPlan(plans[2].ID), plandtos[2]);
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
      var plandto = CA<PlanDto>();
      var plan = CA<Plan>();
      var config = new HttpConfiguration();
      using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/plans")) {
        var routeData = new HttpRouteData(config.Routes.MapHttpRoute("defaultapi", "api/{controller}/{id}"), new HttpRouteValueDictionary { { "controller", "plans" } });
        mController.ControllerContext = new HttpControllerContext(config, routeData, request);
        mController.Request = request;
        mController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        mController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

        mMapper.Setup(m => m.Map<Plan>(plandto)).Returns(plan);
        mRepo.Setup(r => r.Add(It.Is<Plan>(s => IsEqual(s, plan))));
        mMapper.Setup(m => m.Map<PlanDto>(plan)).Returns(plandto);
        using (var response = mController.PostPlan(plandto)) {
          Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
          Assert.That(response.Headers.Location.AbsoluteUri, Is.EqualTo("http://localhost/api/plans/" + plandto.ID));
          var result = response.Content.ReadAsStringAsync();
          result.Wait();
          var actual = JsonConvert.DeserializeObject<PlanDto>(result.Result);
          Compare(actual, plandto);
        }
      }
    }

    [Test]
    public void TestPutPlan() {
      var plandto = CA<PlanDto>();
      var plan = CA<Plan>();
      mMapper.Setup(m => m.Map<Plan>(plandto)).Returns(plan);
      mRepo.Setup(r => r.Update(plan));
      mController.PutPlan(plandto);
    }

    [SetUp]
    public void DoSetup() {
      mMapper = Mok<IObjectMapper>();
      mRepo = Mok<IPlanRepository>();
      mController = new PlanController(mRepo.Object, mMapper.Object);
    }

    private PlanController mController;
    private Mock<IPlanRepository> mRepo;
    private Mock<IObjectMapper> mMapper;
  }
}