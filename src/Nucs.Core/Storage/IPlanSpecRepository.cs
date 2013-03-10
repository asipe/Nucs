using System.Collections.Generic;
using Nucs.Core.Model.Internal;

namespace Nucs.Core.Storage {
  public interface IPlanSpecRepository {
    IEnumerable<Plan> List();
    void Add(Plan plan);
    void Delete(string id);
    void Update(Plan plan);
  }
}