using System;
using Nucs.Core.App;
using Nucs.Core.App.Command;
using Nucs.Core.App.Controller;
using Nucs.Core.App.UI;
using Nucs.Core.App.UI.CLI;

namespace Nucs.Console {
  internal class Program {
    private sealed class TempHandler : IHandler {
      public TempHandler(IController controller) {
        mController = controller;
      }

      public void Handle(string command) {
        if (command.StartsWith("exit"))
          mController.Terminate();
        else
          System.Console.WriteLine("Unknown Command: {0}", command);
      }

      private readonly IController mController;
    }

    private static void Main(string[] args) {
      try {
        Execute(args);
      } catch (Exception ex) {
        System.Console.WriteLine(ex);
      }
    }

    private static void Execute(string[] args) {
      var controller = new SystemController();
      var cli = new CommandLineInterface(System.Console.In, System.Console.Out);
      var handler = new TempHandler(controller);
      var pump = new MessagePump(cli, handler);
      var driver = new Driver(controller, pump);
      driver.Start();
    }
  }
}