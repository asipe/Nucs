using System.Collections.Generic;
using System.Web.Http;
using Nucs.Core.Model.External;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public IEnumerable<PlanDetail> GetAll() {
      return new PlanDetail[0];
    }
  }
}