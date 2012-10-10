using Nucs.Core.App.Controller;
using Nucs.Core.App.UI;

namespace Nucs.Core.App {
  public class Driver {
    public Driver(IController controller, IMessagePump pump) {
      mController = controller;
      mPump = pump;
    }

    public void Start() {
      while (!mController.Terminated)
        mPump.Execute();
    }

    private readonly IController mController;
    private readonly IMessagePump mPump;
  }
}