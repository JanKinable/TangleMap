namespace TangleMap.Model;

public interface IGraphModelRenderer
{
    string Render(IEnumerable<Project> projects);
}
