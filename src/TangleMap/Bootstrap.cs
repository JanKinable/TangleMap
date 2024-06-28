using TangleMap.Model;

namespace TangleMap;

public interface IBootstrap
{
    Task RunAsync(CancellationToken cancellationToken);
}

public class Bootstrap : IBootstrap
{
    private readonly IProjectDependencyCollector _projectDependencyCollector;
    private readonly IAnalyser _analyser;
    private readonly IEnumerable<IGraphModelRenderer> _renderers;
    private readonly IImageGenerator _generator;
    private readonly Options _options;

    public Bootstrap(
        IProjectDependencyCollector projectDependencyCollector,
        IAnalyser analyser,
        IEnumerable<IGraphModelRenderer> renderers,
        IImageGenerator generator,
        Options options)
    {
        _projectDependencyCollector = projectDependencyCollector;
        _analyser = analyser;
        _renderers = renderers;
        _generator = generator;
        _options = options;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var projects = _projectDependencyCollector.BuildDependencyGraph();

        AnalysisReport? report = null;
        if(_options.IncludePackages)
        {
            report = _analyser.AnalyzePackages(projects);
        }

        var modelType = _options.GetModelType();
        if(modelType == ModelType.Undefined) 
            throw new InvalidOperationException($"{_options.OutputType} is unkown.");

        var renderer = _renderers.Single(x => x.ModelType == modelType);

        Console.WriteLine($"Rendering the model...");
        var model = renderer.Render(projects, report, _options.IncludePackages);
        Console.WriteLine($"Done rendering the model.");

        if (_options.SaveRenderModel)
        {
            var diagramOutput = $"{_options.Output}/{_options.Model}_model.txt";
            Console.WriteLine($"Saving rendered model to {diagramOutput}");
            using var streamWriter = new StreamWriter(diagramOutput);
            streamWriter.Write(model);
            streamWriter.Close();
        }

        Console.WriteLine($"Generating the image...");
        await _generator.GenerateImage(model, renderer.ModelType);
        Console.WriteLine($"Done generating the image.");
    }
}
