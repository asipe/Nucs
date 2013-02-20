using System.Collections.Generic;
using System.Linq;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;

namespace Nucs.Core.Mapping {
  public static class PlanMapper {
    public static PlanSpec ToSpec(Plan plan) {
      return ToSpec(new[] { plan }).First();
    }

    public static IEnumerable<PlanSpec> ToSpec(IEnumerable<Plan> plans) {
      return plans
        .Select(plan => new PlanSpec {
                                       ID = plan.ID,
                                       Assembly = plan.Assembly,
                                       Executable = plan.Executable,
                                       Run = plan.Run
                                     });
    }

    public static Plan ToPlan(PlanSpec spec) {
      return ToPlan(new[] {spec}).First();
    }

    public static IEnumerable<Plan> ToPlan(IEnumerable<PlanSpec> specs) {
      return specs
        .Select(spec => new Plan {
                                   ID = spec.ID,
                                   Assembly = spec.Assembly,
                                   Executable = spec.Executable,
                                   Run = spec.Run
                                 });
    }
  }
}