using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using SupaCharge.Testing;

namespace Nucs.UnitTests {
  [TestFixture]
  public abstract class NucsBaseTestCase : BaseTestCase {
    protected void Compare(object actual, object expected) {
      Assert.That(IsEqual(actual, expected), Is.True, _ObjectComparer.DifferencesString);
    }

    protected static bool IsEqual(object actual, object expected) {
      return _ObjectComparer.Compare(actual, expected);
    }

    private static readonly CompareObjects _ObjectComparer = new CompareObjects();
  }
}