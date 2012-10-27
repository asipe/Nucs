using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.App.Controllers.Js {
  public class JsController : ApiController {
    public JsController(IFile file, string assetDir) {
      mFile = file;
      mAssetDir = assetDir;
    }

    public HttpResponseMessage Get() {
      var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(LoadJs())};
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
      return response;
    }

    private string LoadJs() {
      return mFile.ReadAllText(Path.Combine(mAssetDir, @"scripts\nucs.js"));
    }

    private readonly IFile mFile;
    private readonly string mAssetDir;
  }
}