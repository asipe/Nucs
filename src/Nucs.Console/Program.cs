using MadCat.Core;
using Nucs.App.Main;

namespace Nucs.Console {
  internal class Program {
    private static void Main() {
      new Runner().Start(new AppConfig());
    }
  }
}