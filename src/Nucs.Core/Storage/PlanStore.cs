using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nucs.Core.Model;
using Nucs.Core.Serialization;
using SupaCharge.Core.IOAbstractions;

namespace Nucs.Core.Storage {
  public class PlanStore : IPlanStore {
    public PlanStore(string storePath, IFile file, IDirectory directory, ISerializer serializer) {
      mStorePath = storePath;
      mFile = file;
      mDirectory = directory;
      mSerializer = serializer;
    }

    public IEnumerable<Plan> List() {
      return mDirectory
        .GetFiles(mStorePath, "*.json")
        .Select(path => mFile.ReadAllText(Path.Combine(mStorePath, path)))
        .Select(json => mSerializer.Deserialize<Plan>(json))
        .ToArray();
    }

    public void Add(Plan plan) {
      mFile.WriteAllText(Path.Combine(mStorePath, BuildPlanFileName(plan)), mSerializer.Serialize(plan));
    }

    private static string BuildPlanFileName(Plan plan) {
      return string.Format("{0}.json", plan.ID);
    }

    private readonly IDirectory mDirectory;
    private readonly IFile mFile;
    private readonly ISerializer mSerializer;
    private readonly string mStorePath;
  }
}