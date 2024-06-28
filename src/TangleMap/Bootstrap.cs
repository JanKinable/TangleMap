using System.Security.AccessControl;
using TangleMap.Model;
using static System.Net.Mime.MediaTypeNames;

namespace TangleMap;

public interface IBootstrap
{
    Task RunAsync(CancellationToken cancellationToken);
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

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var projects = _projectDependencyCollector.BuildDependencyGraph();

        var modelType = _options.GetModelType();
        if(modelType == ModelType.Undefined) 
            throw new InvalidOperationException($"{_options.OutputType} is unkown.");

        var renderer = _renderers.Single(x => x.ModelType == modelType);

        var model = renderer.Render(projects, _options.IncludePackages);
        if (_options.SaveRenderModel)
        {
            var diagramOutput = $"{_options.Output}/model.txt";
            using var streamWriter = new StreamWriter(diagramOutput);
            streamWriter.Write(model);
            streamWriter.Close();
        }

        await _generator.GenerateImage(model, renderer.ModelType);
    }
}
