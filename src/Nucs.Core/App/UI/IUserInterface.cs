namespace Nucs.Core.App.UI {
  public interface IUserInterface {
    void Write(string msg);
    void WriteLine(string msg);
    string ReadLine();
  }
}