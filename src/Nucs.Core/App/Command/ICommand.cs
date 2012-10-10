namespace Nucs.Core.App.Command {
  public interface ICommand {
    string Name{get;}
    void Execute(string[] args);
  }
}