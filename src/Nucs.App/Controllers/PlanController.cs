using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nucs.Core.Mapping;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;
using Nucs.Core.Storage;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public PlanController(IPlanSpecRepository repo, IObjectMapper mapper) {
      mRepo = repo;
      mMapper = mapper;
    }

    public IEnumerable<PlanDto> GetAllPlans() {
      return mMapper
        .Map<PlanDto[]>(mRepo.List());
    }

    public PlanDto GetPlan(string id) {
      return mMapper
        .Map<PlanDto>(mRepo.List()
                        .First(plan => plan.ID == id));
    }

    public HttpResponseMessage PostPlan(PlanDto plan) {
      var spec = mMapper.Map<PlanSpec>(plan);
      mRepo.Add(spec);
      plan = mMapper.Map<PlanDto>(spec);

      var response = Request.CreateResponse(HttpStatusCode.Created, plan);
      response.Headers.Location = new Uri(Url.Link("defaultapi", new {id = plan.ID}));
      return response;
    }

    public void PutPlan(PlanDto plan) {
      mRepo.Update(mMapper.Map<PlanSpec>(plan));
    }

    public void DeletePlan(string id) {
      mRepo.Delete(id);
    }

    private readonly IObjectMapper mMapper;
    private readonly IPlanSpecRepository mRepo;
  }
}