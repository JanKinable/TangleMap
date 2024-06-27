using Microsoft.Extensions.Configuration;
using TangleMap.Model;

namespace TangleMap.Mermaid;

public class GraphModelRenderer : IGraphModelRenderer
{
    private readonly MermaidOptions _options;
    public GraphModelRenderer(IConfiguration configuration)
    {
        _options = new ();
        configuration.GetSection("Mermaid").Bind(_options);
    }

    public string Render(IEnumerable<Project> projects)
    {
        throw new NotImplementedException();
    }
}
