using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using SupaCharge.Testing;

namespace Nucs.UnitTests {
  [TestFixture]
  public abstract class NucsBaseTestCase : BaseTestCase {
    protected void Compare(object actual, object expected) {
      Assert.That(_ObjectComparer.Compare(actual, expected), Is.True, _ObjectComparer.DifferencesString);
    }

    private static readonly CompareObjects _ObjectComparer = new CompareObjects();
  }
}