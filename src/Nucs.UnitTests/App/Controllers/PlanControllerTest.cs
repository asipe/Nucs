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
      var specs = BA<PlanSpec>();
      mRepo.Setup(r => r.List()).Returns(specs);
      mMapper.Setup(m => m.Map<PlanDto[]>(specs)).Returns(BA<PlanDto>());
      Assert.That(mController.GetAllPlans(), Is.Empty);
    }

    [Test]
    public void TetGetAllPlansWithPlans() {
      var plans = CM<PlanDto>();
      var specs = CM<PlanSpec>();
      mRepo.Setup(r => r.List()).Returns(specs);
      mMapper.Setup(m => m.Map<PlanDto[]>(specs)).Returns(plans);
      Compare(mController.GetAllPlans().ToArray(), plans);
    }

    [Test]
    public void TestGetSinglePlanByIDExists() {
      var plans = CM<PlanDto>();
      var specs = CM<PlanSpec>();
      mRepo.Setup(r => r.List()).Returns(specs);
      mMapper.Setup(m => m.Map<PlanDto>(specs[0])).Returns(plans[0]);
      Compare(mController.GetPlan(specs[0].ID), plans[0]);
      mMapper.Setup(m => m.Map<PlanDto>(specs[2])).Returns(plans[2]);
      Compare(mController.GetPlan(specs[2].ID), plans[2]);
    }

    [Test]
    public void TestGetSinglePlanByIDWhichDoesNotExistThrows() {
      var specs = CM<PlanSpec>();
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
      var plan = CA<PlanDto>();
      var spec = CA<PlanSpec>();
      var config = new HttpConfiguration();
      using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/plans")) {
        var routeData = new HttpRouteData(config.Routes.MapHttpRoute("defaultapi", "api/{controller}/{id}"), new HttpRouteValueDictionary { { "controller", "plans" } });
        mController.ControllerContext = new HttpControllerContext(config, routeData, request);
        mController.Request = request;
        mController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        mController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

        mMapper.Setup(m => m.Map<PlanSpec>(plan)).Returns(spec);
        mRepo.Setup(r => r.Add(It.Is<PlanSpec>(s => IsEqual(s, spec))));
        mMapper.Setup(m => m.Map<PlanDto>(spec)).Returns(plan);
        using (var response = mController.PostPlan(plan)) {
          Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
          Assert.That(response.Headers.Location.AbsoluteUri, Is.EqualTo("http://localhost/api/plans/" + plan.ID));
          var result = response.Content.ReadAsStringAsync();
          result.Wait();
          var actual = JsonConvert.DeserializeObject<PlanDto>(result.Result);
          Compare(actual, plan);
        }
      }
    }

    [Test]
    public void TestPutPlan() {
      var plan = CA<PlanDto>();
      var spec = CA<PlanSpec>();
      mMapper.Setup(m => m.Map<PlanSpec>(plan)).Returns(spec);
      mRepo.Setup(r => r.Update(spec));
      mController.PutPlan(plan);
    }

    [SetUp]
    public void DoSetup() {
      mMapper = Mok<IObjectMapper>();
      mRepo = Mok<IPlanSpecRepository>();
      mController = new PlanController(mRepo.Object, mMapper.Object);
    }

    private PlanController mController;
    private Mock<IPlanSpecRepository> mRepo;
    private Mock<IObjectMapper> mMapper;
  }
}