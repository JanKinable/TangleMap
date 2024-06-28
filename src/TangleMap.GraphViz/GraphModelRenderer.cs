using System.Text;
using TangleMap.Model;

namespace TangleMap.GraphViz;

public class GraphModelRenderer : IGraphModelRenderer
{
    public ModelType ModelType => ModelType.GraphViz;

    public string Render(IEnumerable<Project> projects, AnalysisReport report, bool includePackages)
    {
        var sb = new StringBuilder();

        sb.AppendLine("digraph G {");
        sb.AppendLine("    rankdir=\"LR\"");
        sb.AppendLine();

        foreach (var project in projects.OrderByDescending(x => x.ProjectDependencies.Count))
        {
            var idName = project.ProjectName.Name.Replace('.', '_');
            var labelName = project.ProjectName.Name;
            sb.AppendLine($"    {idName}[style=\"filled\", shape=box, fillcolor = \"#0000bb\", fontcolor  = \"white\", label=\"{labelName}\"]");
        }
        
        if (includePackages)
        {
            var packages = projects.SelectMany(x => x.Packages).Distinct();
            foreach (var package in packages)
            {
                var labelName = $"{package.Name} {package.Version}";
                var idName = $"{package.Name}_{package.Version}".Replace('.', '_');
                if (report.SuspiciousPackages.Any(x => x.PackageName == package.Name))
                {
                    sb.AppendLine($"    {idName}[style=\"filled,rounded\", shape=box, fillcolor = \"#ff8400\", fontcolor  = \"white\", label=\"{labelName}\"]");
                }
                else
                {
                    sb.AppendLine($"    {idName}[style=\"filled,rounded\", shape=box, fillcolor = \"#0077ff\", fontcolor  = \"white\", label=\"{labelName}\"]");
                }
                    
            }
        }


        sb.AppendLine();
        foreach (var project in projects)
        {
            foreach (var dependency in project.ProjectDependencies)
            {
                sb.AppendLine($"  {project.ProjectName.Name.Replace('.', '_')} -> {dependency.Name.Replace('.','_')}");
            }
            if (includePackages)
            {
                foreach (var package in project.Packages)
                {
                    var packageIdName = $"{package.Name}_{package.Version}".Replace('.', '_');
                    sb.AppendLine($"  {project.ProjectName.Name.Replace('.', '_')} -> {packageIdName} [style=\"dotted\"]");
                }
            }
        }

        sb.AppendLine();
        sb.AppendLine("}");





        return sb.ToString();
    }
}
