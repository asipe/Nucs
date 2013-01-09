using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nucs.Core.Model;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public PlanController(IPlanStore store) {
      mStore = store;
    }

    public IEnumerable<PlanDetail> GetPlans() {
      return mStore
        .List()
        .Select(BuildPlanDetail)
        .ToArray();
    }

    public HttpResponseMessage DeletePlan(string id) {
      mStore.Delete(id);
      return new HttpResponseMessage {StatusCode = HttpStatusCode.Accepted};
    }

    public HttpResponseMessage CreatePlan(PlanDetail detail) {
      return new HttpResponseMessage {StatusCode = HttpStatusCode.Created};
    }

    private static PlanDetail BuildPlanDetail(Plan plan) {
      return new PlanDetail {
                              Assembly = plan.Assembly,
                              Executable = plan.Executable,
                              Run = plan.Run
                            };
    }

    private readonly IPlanStore mStore;
  }
}