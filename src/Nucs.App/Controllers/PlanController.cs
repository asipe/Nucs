using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nucs.Core.Mapping;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public PlanController(IPlanSpecRepository repo) {
      mRepo = repo;
    }

    public IEnumerable<Plan> GetAllPlans() {
      return PlanMapper.ToPlan(mRepo.List());
    }

    public Plan GetPlan(string id) {
      return PlanMapper.ToPlan(mRepo.List().Where(plan => plan.ID == id)).First();
    }

    public HttpResponseMessage PostPlan(Plan plan) {
      var spec = PlanMapper.ToSpec(plan);
      mRepo.Add(spec);
      plan = PlanMapper.ToPlan(spec);

      var response = Request.CreateResponse(HttpStatusCode.Created, plan);
      response.Headers.Location = new Uri(Url.Link("defaultapi", new {id = plan.ID}));
      return response;
    }

    public void PutPlan(Plan plan) {
      mRepo.Update(PlanMapper.ToSpec(plan));
    }

    public void DeletePlan(string id) {
      mRepo.Delete(id);
    }

    private readonly IPlanSpecRepository mRepo;
  }
}