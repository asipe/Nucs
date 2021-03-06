﻿namespace Nucs.Core.Serialization {
  public interface ISerializer {
    string Serialize(object obj);
    T Deserialize<T>(string data);
  }
}