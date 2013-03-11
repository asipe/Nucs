using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Nucs.Core.Model.Internal;
using SupaCharge.Testing;

namespace Nucs.UnitTests.Core.Model.Internal {
  [TestFixture]
  public class EventQueueTest : BaseTestCase {
    [Test]
    public void TestEnqueuedItemsIsAvailableForDequeue() {
      var evt = CA<Event>();
      mQueue.Enqueue(evt);
      Assert.That(mQueue.Dequeue(1000), Is.EqualTo(evt));
    }

    [Test]
    public void TestDequeueBlocksUntilItemQueued() {
      using (var task = Task.Factory.StartNew(() => mQueue.Dequeue(1000))) {
        Thread.Sleep(100);
        Assert.That(task.IsCompleted, Is.False);
        var evt = CA<Event>();
        mQueue.Enqueue(evt);
        Assert.That(task.Result, Is.EqualTo(evt));
        Assert.That(task.IsCompleted, Is.True);
      }
    }

    [Test]
    public void TestDequeueWillTimeoutAndReturnNullIfNothingAdded() {
      var timer = new Stopwatch();
      timer.Start();
      using (var task = Task.Factory.StartNew(() => mQueue.Dequeue(150))) {
        task.Wait();
        Assert.That(timer.ElapsedMilliseconds, Is.GreaterThan(125).And.LessThan(175));
        Assert.That(task.Result, Is.Null);
      }
    }

    [Test]
    public void TestShutdownWillRelease() {
      using (var task = Task.Factory.StartNew(() => mQueue.Dequeue(int.MaxValue))) {
        mQueue.Shutdown();
        task.Wait();
        Assert.That(task.Result, Is.Null);
      }
    }

    [Test]
    public void TestShutdownBeforeDequeueDoesNotLock() {
      mQueue.Shutdown();
      Assert.That(mQueue.Dequeue(int.MaxValue), Is.Null);
    }

    [Test]
    public void TestItemInQueueWhenDequeuedDoesNotBlock() {
      var evt = CA<Event>();
      mQueue.Enqueue(evt);
      Assert.That(mQueue.Dequeue(1000), Is.EqualTo(evt));
    }

    [Test]
    public void TestMultipleItemsInQueue() {
      var evts = CM<Event>();
      Array.ForEach(evts, evt => mQueue.Enqueue(evt));
      Array.ForEach(evts, evt => Assert.That(mQueue.Dequeue(1000), Is.EqualTo(evt)));
    }

    [SetUp]
    public void DoSetup() {
      mQueue = new EventQueue();
    }

    private EventQueue mQueue;
  }
}