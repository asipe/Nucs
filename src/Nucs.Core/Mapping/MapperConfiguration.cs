using AutoMapper;
using Nucs.Core.Model.External;
using Nucs.Core.Model.Internal;

namespace Nucs.Core.Mapping {
  public class MapperConfiguration {
    public void Configure() {
      Mapper.CreateMap<PlanSpec, Plan>();
      Mapper.CreateMap<Plan, PlanSpec>();
    }
  }
}