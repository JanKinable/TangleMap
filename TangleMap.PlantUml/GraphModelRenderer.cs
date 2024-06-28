using System.Text;
using TangleMap.Model;

namespace TangleMap.PlantUml;

public class GraphModelRenderer : IGraphModelRenderer
{
    public ModelType ModelType => ModelType.PlantUml;

    public string Render(IEnumerable<Project> projects, bool includePackages)
    {
        var sb = new StringBuilder();

        sb.AppendLine("@startuml");
        sb.AppendLine();
        sb.AppendLine("left to right direction");
        sb.AppendLine();
        var styleBlock = @"
<style>
 .package {
   BackgroundColor #07F
   FontColor #fff
   FontStyle italic
   RoundCorner 50
 }
 .assembly {
   BackgroundColor #00b
   FontColor #fff
   RoundCorner 1
 }
</style>
";
        sb.AppendLine(styleBlock);
        sb.AppendLine();

        foreach (var project in projects.OrderByDescending(x => x.ProjectDependencies.Count))
        {
            sb.AppendFormat("rectangle {0} <<assembly>>", project.ProjectName.Name);
            sb.AppendLine();
        }

        if (includePackages)
        {
            var packages = projects.SelectMany(x => x.Packages).Distinct();
            foreach (var package in packages)
            {
                sb.AppendFormat("rectangle {0}_{1} <<package>>", package.Name, package.Version); 
                sb.AppendLine();
            }
        }


        sb.AppendLine();
        foreach (var project in projects)
        {
            foreach (var dependency in project.ProjectDependencies)
            {
                sb.AppendLine($"  {project.ProjectName.Name} --> {dependency.Name}");
            }
            if (includePackages)
            {
                foreach (var package in project.Packages)
                {
                    sb.AppendLine($"  {project.ProjectName.Name} -.-> {package.Name}_{package.Version}");
                }
            }
        }

        sb.AppendLine();
        sb.AppendLine("@enduml");





        return sb.ToString();
    }
}
