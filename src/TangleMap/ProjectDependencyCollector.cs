using Microsoft.Build.Construction;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TangleMap.Model;

namespace TangleMap;

public interface IProjectDependencyCollector 
{
    IEnumerable<Project> BuildDependencyGraph();
}

public class ProjectDependencyCollector : IProjectDependencyCollector
{
    private readonly Options _options;

    public ProjectDependencyCollector(Options options)
    {
        _options = options;
    }

    public IEnumerable<Project> BuildDependencyGraph()
    {
        var solutionPath = Path.GetDirectoryName(_options.Solution)!;

        // Load the solution
        SolutionFile solution = SolutionFile.Parse(_options.Solution);

        // Loop through each project in the solution
        foreach (ProjectInSolution project in solution.ProjectsInOrder
            .Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat))
        {
            if (_options.IsFiltered(project.ProjectName)) continue;

            var projectName = new ProjectName(project.ProjectName, project.AbsolutePath);
            var projectDependencies = new Project(projectName);

            var csprojDoc = XDocument.Load(project.AbsolutePath);
            var references = csprojDoc.Descendants("ProjectReference");
            foreach (XElement reference in references)
            {
                var relPath = reference.Attribute("Include")!.Value;
                var combindedPath = Path.Combine(solutionPath, "..", relPath);
                var absolutePath = Path.GetFullPath(combindedPath);
                var dependency = new ProjectName(absolutePath);
                projectDependencies.ProjectDependencies.Add(dependency);
            }

            if (_options.IncludePackages)
            {
                var packages = csprojDoc.Descendants("PackageReference");
                foreach(XElement packageReference in packages)
                {
                    var name = packageReference.Attribute("Include")!.Value;
                    var versionAttr = packageReference.Attribute("Version")!;
                    var version = ResolveVersion(versionAttr.Value);
                    var package = new Package { Name = name, Version = version };
                    projectDependencies.Packages.Add(package);
                }
            }

            yield return projectDependencies;
        }

    }

    private static Version ResolveVersion(string? value)
    {
        if (value == null)
            return new Version(0, 0, 0, 0);

        if (Version.TryParse(value, out Version? version))
        {
            return version;
        }

        //try resolve format 1.1.0-CI-20230125-093829
        var stripped = value.Split('-').First();
        if (Version.TryParse(stripped, out Version? versionStripped))
        {
            return versionStripped;
        }

        return new Version(0, 0, 0, 0);
    }
}
