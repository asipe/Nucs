using System.Collections.Generic;
using System.Web.Http;
using Nucs.Core.Model;

namespace Nucs.App.Controllers {
  public class PlanController : ApiController {
    public IEnumerable<Plan> GetAll() {
      return new Plan[0];
    } 
  }
}