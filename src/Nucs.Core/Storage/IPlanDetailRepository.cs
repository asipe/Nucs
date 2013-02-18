using System.Collections.Generic;
using Nucs.Core.Model.External;

namespace Nucs.Core.Storage {
  public interface IPlanDetailRepository {
    IEnumerable<Plan> List();
    void Add(Plan plan);
    void Delete(string id);
  }
}