using AutoMapper;

namespace Nucs.Core.Mapping {
  public class ObjectMapper : IObjectMapper {
    public TDestination Map<TDestination>(object source) {
      return Mapper.Map<TDestination>(source);
    }
  }
}