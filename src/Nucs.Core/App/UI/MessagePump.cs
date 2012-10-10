using Nucs.Core.App.Command;

namespace Nucs.Core.App.UI {
  public class MessagePump : IMessagePump {
    public MessagePump(IUserInterface ui, IHandler handler) {
      mUI = ui;
      mHandler = handler;
    }

    public void Execute() {
      mUI.Write("nucs>");
      mHandler.Handle(mUI.ReadLine());
    }

    private readonly IHandler mHandler;
    private readonly IUserInterface mUI;
  }
}