using System.Collections.Generic;
using Nucs.Core.Model;

namespace Nucs.Core.Storage {
  public interface IPlanStore {
    IEnumerable<Plan> List();
    void Add(Plan plan);
    void Delete(Plan plan);
  }
}