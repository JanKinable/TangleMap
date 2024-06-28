﻿using System.Text;
using TangleMap.Model;

namespace TangleMap.Mermaid;

public class GraphModelRenderer : IGraphModelRenderer
{
    public ModelType ModelType => ModelType.Mermaid;

    public string Render(IEnumerable<Project> projects, bool includePackages)
    {
        var sb = new StringBuilder();

        sb.AppendLine("%%{init: {'theme':'" + "default" + "'}}%%");
        sb.AppendLine("graph LR");
        
        sb.AppendLine("  classDef assembly fill:#00b,color:#fff,stroke:#000;");
        sb.AppendLine("  classDef package fill:#07F,color:#fff,font-style:italic,stroke:#000;");
        sb.AppendLine();

        foreach (var project in projects.OrderByDescending(x => x.ProjectDependencies.Count))
        {
            sb.AppendFormat("  {0}[\"{0}\"]:::assembly;", project.ProjectName.Name);
            sb.AppendLine();
        }

        if (includePackages)
        {
            var packages = projects.SelectMany(x => x.Packages).Distinct();
            foreach (var package in packages)
            {
                sb.AppendFormat("  {0}_{1}([\"{0} {1}\"]):::package;", package.Name, package.Version); 
                sb.AppendLine();
            }
        }


        sb.AppendLine();
        foreach (var project in projects)
        {
            foreach (var dependency in project.ProjectDependencies)
            {
                sb.AppendLine($"  {project.ProjectName.Name} --> {dependency.Name};");
            }
            if (includePackages)
            {
                foreach (var package in project.Packages)
                {
                    sb.AppendLine($"  {project.ProjectName.Name} -.-> {package.Name}_{package.Version};");
                }
            }
        }

        sb.AppendLine();

        



        return sb.ToString();
    }
}
