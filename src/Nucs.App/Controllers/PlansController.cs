using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.App.Controllers {
  public class PlansController : ApiController {
    public PlansController(IPlanDetailRepository repo) {
      mRepo = repo;
    }

    public IEnumerable<Plan> GetAllPlans() {
      return mRepo.List();
    }

    public Plan GetPlan(string id) {
      return mRepo
        .List()
        .First(plan => plan.ID == id);
    }

    public HttpResponseMessage PostPlan(Plan plan) {
      var response = Request.CreateResponse(HttpStatusCode.Created, new Plan());
      response.Headers.Location = new Uri(Url.Link("defaultapi", new { id = "abc" }));
      return response;
    }

    public void PutPlan(Plan plan) {
      mRepo.Delete(plan.ID);
      mRepo.Add(plan);
    }

    public void DeletePlan(string id) {
      mRepo.Delete(id);
    }

    private readonly IPlanDetailRepository mRepo;
  }
}