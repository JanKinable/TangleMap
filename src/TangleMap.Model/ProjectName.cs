namespace TangleMap.Model;

public class ProjectName
{
    public ProjectName(string path)
    {
        Path = path;
        if (path.Length > 0)
        {
            char splitter = path.Contains('/') ? '/' : '\\';
            var csproj = path.Split(splitter).Last();
            Name = csproj.Replace(".csproj", "");
        }
        else
        {
            Name = string.Empty;
        }
    }

    public ProjectName(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public string Path { get; private set; }
    public string Name { get; private set; }
}