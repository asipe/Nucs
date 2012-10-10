namespace Nucs.Core.App.Controller {
  public interface IController {
    bool Terminated { get; }
    void Terminate(); 
  }
}