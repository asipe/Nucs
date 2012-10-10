using System.IO;

namespace Nucs.Core.App.UI.CLI {
  public class CommandLineInterface : IUserInterface {
    public CommandLineInterface(TextReader instrm, TextWriter outstrm) {
      mInstrm = instrm;
      mOutstrm = outstrm;
    }

    public void Write(string msg) {
      mOutstrm.Write(msg);
    }

    public void WriteLine(string msg) {
      mOutstrm.WriteLine(msg);
    }

    public string ReadLine() {
      return mInstrm.ReadLine();
    }

    private readonly TextReader mInstrm;
    private readonly TextWriter mOutstrm;
  }
}