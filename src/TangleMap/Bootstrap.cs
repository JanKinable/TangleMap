using TangleMap.Model;

namespace TangleMap;

public interface IBootstrap
{
    Task RunAsync();
}

public class Bootstrap : IBootstrap
{
    private readonly IProjectDependencyCollector _projectDependencyCollector;
    private readonly IEnumerable<IGraphModelRenderer> _renderers;
    private readonly IImageGenerator _generator;
    private readonly Options _options;

    public Bootstrap(
        IProjectDependencyCollector projectDependencyCollector,
        IEnumerable<IGraphModelRenderer> renderers,
        IImageGenerator generator,
        Options options)
    {
        _projectDependencyCollector = projectDependencyCollector;
        _renderers = renderers;
        _generator = generator;
        _options = options;
    }

    public async Task RunAsync()
    {
        var projects = _projectDependencyCollector.BuildDependencyGraph();

        foreach (var renderer in _renderers)
        {
            var model = renderer.Render(projects, _options.IncludePackages);
            await _generator.GenerateImage(model, renderer.Model);
        }
    }
}
