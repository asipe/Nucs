using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Nucs.App.Controllers.Index {
  public class IndexController : ApiController {
    public HttpResponseMessage Get() {
      var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StreamContent(File.Open(@"views\index\index.html", FileMode.Open))};
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
      return response;
    }
  }
}