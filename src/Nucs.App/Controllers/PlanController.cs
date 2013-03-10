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
    public PlanController(IPlanRepository repo, IObjectMapper mapper) {
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

    public HttpResponseMessage PostPlan(PlanDto plandto) {
      var plan = mMapper.Map<Plan>(plandto);
      mRepo.Add(plan);
      plandto = mMapper.Map<PlanDto>(plan);

      var response = Request.CreateResponse(HttpStatusCode.Created, plandto);
      response.Headers.Location = new Uri(Url.Link("defaultapi", new {id = plandto.ID}));
      return response;
    }

    public void PutPlan(PlanDto plandto) {
      mRepo.Update(mMapper.Map<Plan>(plandto));
    }

    public void DeletePlan(string id) {
      mRepo.Delete(id);
    }

    private readonly IObjectMapper mMapper;
    private readonly IPlanRepository mRepo;
  }
}