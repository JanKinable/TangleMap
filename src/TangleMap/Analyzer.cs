using TangleMap.Model;

namespace TangleMap;

public interface IAnalyser
{
    AnalysisReport AnalyzePackages(IEnumerable<Project> projects);
}

public class Analyzer : IAnalyser
{
    public AnalysisReport AnalyzePackages(IEnumerable<Project> projects)
    {
        List<SuspiciousPackage> suspiciousPackages = [];

        var projectPackages = (from prj in projects
                               from pac in prj.Packages
                               select new ProjectPackage(prj.ProjectName.Name, pac.Name, pac.Version.ToString())).ToList();

        var pacGroups = projectPackages.GroupBy(x => x.PackageName);
        foreach (var pacGroup in pacGroups)
        {
            var versions = pacGroup.GroupBy(x => x.PackageVersion);
            if (versions.Count() > 1)
            {
                var susPack = new SuspiciousPackage { PackageName = pacGroup.Key };
                susPack.Reason = SuspiciousPackageReason.MultipleVersion;
                foreach (var ver in versions)
                {
                    foreach (var version in ver)
                    {
                        susPack.AdditionalInformation.Add($"{version.PackageVersion} used in {version.ProjectName}");
                    }
                }
                suspiciousPackages.Add(susPack);
            }
        }

        return new AnalysisReport { SuspiciousPackages = suspiciousPackages };
    }

    private record ProjectPackage(string ProjectName, string PackageName, string PackageVersion);

}
