using System.Linq;
using NUnit.Framework;
using Nucs.Core.Mapping;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;

namespace Nucs.UnitTests.Core.Mapping {
  [TestFixture]
  public class PlanMapperTest : NucsBaseTestCase {
    [Test]
    public void TestMapSinglePlanToSpec() {
      var plan = CA<Plan>();
      var expected = new PlanSpec {
                                    ID = plan.ID,
                                    Assembly = plan.Assembly,
                                    Executable = plan.Executable,
                                    Run = plan.Run
                                  };
      Compare(PlanMapper.ToSpec(plan), expected);
    }

    [Test]
    public void TestMapMultiplePlansToSpecs() {
      var plans = CM<Plan>();
      var expected = BA(new PlanSpec {
                                       ID = plans[0].ID,
                                       Assembly = plans[0].Assembly,
                                       Executable = plans[0].Executable,
                                       Run = plans[0].Run
                                     },
                        new PlanSpec {
                                       ID = plans[1].ID,
                                       Assembly = plans[1].Assembly,
                                       Executable = plans[1].Executable,
                                       Run = plans[1].Run
                                     },
                        new PlanSpec {
                                       ID = plans[2].ID,
                                       Assembly = plans[2].Assembly,
                                       Executable = plans[2].Executable,
                                       Run = plans[2].Run
                                     });
      Compare(PlanMapper.ToSpec(plans).ToArray(), expected);
    }

    [Test]
    public void TestMapSingleSpecToPlan() {
      var spec = CA<PlanSpec>();
      var expected = new Plan {
                                ID = spec.ID,
                                Assembly = spec.Assembly,
                                Executable = spec.Executable,
                                Run = spec.Run
                              };
      Compare(PlanMapper.ToPlan(spec), expected);
    }

    [Test]
    public void TestMapMultipleSpecsToPlans() {
      var specs = CM<PlanSpec>();
      var expected = BA(new Plan {
                                   ID = specs[0].ID,
                                   Assembly = specs[0].Assembly,
                                   Executable = specs[0].Executable,
                                   Run = specs[0].Run
                                 },
                        new Plan {
                                   ID = specs[1].ID,
                                   Assembly = specs[1].Assembly,
                                   Executable = specs[1].Executable,
                                   Run = specs[1].Run
                                 },
                        new Plan {
                                   ID = specs[2].ID,
                                   Assembly = specs[2].Assembly,
                                   Executable = specs[2].Executable,
                                   Run = specs[2].Run
                                 });
      Compare(PlanMapper.ToPlan(specs).ToArray(), expected);
    }
  }
}