using System.Collections.Generic;
using Nucs.Core.Model.Internal;

namespace Nucs.Core.Storage {
  public interface IPlanRepository {
    IEnumerable<Plan> List();
    void Add(Plan plan);
    void Delete(string id);
    void Update(Plan plan);
  }
}