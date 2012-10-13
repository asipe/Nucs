using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Controllers.Css {
  public class CssController : ApiController {
    public CssController(IFile file) {
      mFile = file;
    }

    public HttpResponseMessage Get() {
      var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(LoadCss())};
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/css");
      return response;
    }

    private string LoadCss() {
      return mFile.ReadAllText(@"css\nucs.css");
    }

    private readonly IFile mFile;
  }
}