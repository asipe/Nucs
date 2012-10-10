namespace Nucs.Core.App.Controller {
  public class SystemController : IController {
    public bool Terminated{get;private set;}

    public void Terminate() {
      Terminated = true;
    }
  }
}