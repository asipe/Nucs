using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Controllers.Index {
  public class IndexController : ApiController {
    public IndexController(IFile file) {
      mFile = file;
    }

    public HttpResponseMessage Get() {
      var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(LoadHtml())};
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
      return response;
    }

    private string LoadHtml() {
      return mFile.ReadAllText(@"views\index\index.html");
    }

    private readonly IFile mFile;
  }
}