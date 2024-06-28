using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace TangleMap.Model;

public interface IGraphModelRenderer
{
    ModelType ModelType { get; }

    string Render(IEnumerable<Project> projects, bool includePackages);
}
