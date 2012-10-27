using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Controllers.Js {
  public class JsController : ApiController {
    public JsController(IFile file) {
      mFile = file;
    }

    public HttpResponseMessage Get() {
      var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(LoadJs())};
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
      return response;
    }

    private string LoadJs() {
      return mFile.ReadAllText(@"scripts\nucs.js");
    }

    private readonly IFile mFile;
  }
}