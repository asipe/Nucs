using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nucs.Core.Model.External;
using Nucs.Core.Serialization;
using SupaCharge.Core.IOAbstractions;
using SupaCharge.Core.OID;

namespace Nucs.Core.Storage {
  public class PlanDetailRepository : IPlanDetailRepository {
    public PlanDetailRepository(string storePath,
                                IFile file,
                                IDirectory directory,
                                ISerializer serializer,
                                IOIDProvider oidProvider) {
      mStorePath = storePath;
      mFile = file;
      mDirectory = directory;
      mSerializer = serializer;
      mOIDProvider = oidProvider;
    }

    public IEnumerable<Plan> List() {
      return mDirectory
        .GetFiles(mStorePath, "*.json")
        .Select(path => mFile.ReadAllText(BuildPlanFilePath(path)))
        .Select(json => mSerializer.Deserialize<Plan>(json))
        .ToArray();
    }

    public void Add(Plan plan) {
      plan.ID = mOIDProvider.GetID();
      mFile.WriteAllText(BuildPlanFilePath(plan), mSerializer.Serialize(plan));
    }

    public void Delete(string id) {
      mFile.Delete(BuildPlanFilePath(BuildPlanFileName(id)));
    }

    public void Update(Plan plan) {
      mFile.WriteAllText(BuildPlanFilePath(plan), mSerializer.Serialize(plan));
    }

    private string BuildPlanFilePath(Plan plan) {
      return BuildPlanFilePath(BuildPlanFileName(plan));
    }

    private string BuildPlanFilePath(string planFileName) {
      return Path.Combine(mStorePath, planFileName);
    }

    private static string BuildPlanFileName(Plan plan) {
      return BuildPlanFileName(plan.ID);
    }

    private static string BuildPlanFileName(string id) {
      return string.Format("{0}.json", id);
    }

    private readonly IDirectory mDirectory;
    private readonly IFile mFile;
    private readonly IOIDProvider mOIDProvider;
    private readonly ISerializer mSerializer;
    private readonly string mStorePath;
  }
}