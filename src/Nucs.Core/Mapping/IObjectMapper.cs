namespace Nucs.Core.Mapping {
  public interface IObjectMapper {
    TDestination Map<TDestination>(object source);
  }
}