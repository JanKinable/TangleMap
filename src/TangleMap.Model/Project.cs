namespace TangleMap.Model;
public class Project
{
    public Project(ProjectName projectName) => ProjectName = projectName;

    public ProjectName ProjectName { get; private set; }

    public List<ProjectName> ProjectDependencies { get; set; } = [];

    public List<Package> Packages { get; set; } = [];
}

