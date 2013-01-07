using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Nucs.Core.Model;
using Nucs.Core.Model.External;
using Nucs.Core.Storage;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public PlanController(IPlanStore store) {
      mStore = store;
    }

    public IEnumerable<PlanDetail> GetAll() {
      return mStore
        .List()
        .Select(BuildPlanDetail)
        .ToArray();
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