using System.Collections.Generic;
using System.Threading;

namespace Nucs.Core.Model.Internal {
  public class EventQueue {
    public void Enqueue(Event evt) {
      Monitor.Enter(mLock);
      try {
        mQueue.Enqueue(evt);
        Monitor.PulseAll(mLock);
      } finally {
        Monitor.Exit(mLock);
      }
    }

    public Event Dequeue(int waitMillis) {
      Monitor.Enter(mLock);
      try {
        if (!mShutdown && mQueue.Count == 0)
          Monitor.Wait(mLock, waitMillis);
        return mQueue.Count == 0 ? null : mQueue.Dequeue();
      } finally {
        Monitor.Exit(mLock);
      }
    }

    public void Shutdown() {
      Monitor.Enter(mLock);
      try {
        mShutdown = true;
        Monitor.PulseAll(mLock);
      } finally {
        Monitor.Exit(mLock);
      }
    }

    private readonly object mLock = new object();
    private readonly Queue<Event> mQueue = new Queue<Event>();
    private bool mShutdown;
  }
}