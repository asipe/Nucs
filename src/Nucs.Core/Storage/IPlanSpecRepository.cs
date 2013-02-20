using System.Collections.Generic;
using Nucs.Core.Model.Internal;

namespace Nucs.Core.Storage {
  public interface IPlanSpecRepository {
    IEnumerable<PlanSpec> List();
    void Add(PlanSpec plan);
    void Delete(string id);
    void Update(PlanSpec plan);
  }
}