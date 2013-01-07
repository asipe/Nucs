﻿using Newtonsoft.Json;

namespace Nucs.Core.Serialization {
  public class Serializer : ISerializer {
    public string Serialize(object obj) {
      return JsonConvert.SerializeObject(obj);
    }

    public T Deserialize<T>(string data) {
      return JsonConvert.DeserializeObject<T>(data);
    }
  }
}