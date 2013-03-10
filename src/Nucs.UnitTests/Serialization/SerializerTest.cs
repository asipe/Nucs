﻿using NUnit.Framework;
using Nucs.Core.Model.External;
using Nucs.Core.Serialization;

namespace Nucs.UnitTests.Serialization {
  [TestFixture]
  public class SerializerTest : NucsBaseTestCase {
    [Test]
    public void TestBasicSerialization() {
      var serializer = new Serializer();
      var expected = CA<PlanDto>();
      var json = serializer.Serialize(expected);
      Compare(serializer.Deserialize<PlanDto>(json), expected);
    }
  }
}