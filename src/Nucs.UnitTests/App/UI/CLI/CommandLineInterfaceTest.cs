using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Nucs.Core.App.UI.CLI;
using SupaCharge.Testing;

namespace Nucs.UnitTests.App.UI.CLI {
  [TestFixture]
  public class CommandLineInterfaceTest : BaseTestCase {
    [Test]
    public void TestWriteWritesToOut() {
      mUI.Write("a msg");
      Assert.That(mOutBuf.ToString(), Is.EqualTo("a msg"));
    }

    [Test]
    public void TestWriteLineWritesToOutWithEOL() {
      mUI.WriteLine("a msg");
      Assert.That(mOutBuf.ToString(), Is.EqualTo("a msg" + Environment.NewLine));
    }

    [Test]
    public void TestReadLineReadsFromIn() {
      Assert.That(mUI.ReadLine(), Is.EqualTo("cmd a"));
      Assert.That(mUI.ReadLine(), Is.EqualTo("cmd b"));
      Assert.That(mUI.ReadLine(), Is.EqualTo("cmd c"));
    }

    [SetUp]
    public void DoSetup() {
      mIn = new StringReader(string.Join(Environment.NewLine, BA("cmd a", "cmd b", "cmd c")));
      mOutBuf = new StringBuilder();
      mOut = new StringWriter(mOutBuf);
      mUI = new CommandLineInterface(mIn, mOut);
    }

    private StringReader mIn;
    private StringBuilder mOutBuf;
    private StringWriter mOut;
    private CommandLineInterface mUI;
  }
}