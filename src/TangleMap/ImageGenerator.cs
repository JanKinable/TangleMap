using KrokiNet;
using TangleMap.Model;

namespace TangleMap;

public interface IImageGenerator
{
    Task GenerateImage(string model, ModelType sourceType);
}

public class ImageGenerator : IImageGenerator
{
    private readonly Options _options;
    private readonly IKrokiClient _krokiClient;

    public ImageGenerator(Options options, IKrokiClient krokiClient)
    {
        _options = options;
        _krokiClient = krokiClient;
    }

    public async Task GenerateImage(string model, ModelType sourceType)
    {
        try
        {
            var args = new KrokiArguments
            {
                Source = model,
                SourceType = GetDiagramType(sourceType),
                OutputType = GetOutputType(_options.OutputType)
            };

            var res = await _krokiClient.ConvertAsync(args);

            var ext = GetExtensionForOutputType(args.OutputType);
            var diagramOutput =$"{_options.Output}/{sourceType}.{ext}";
            using var fileStream = File.Create(diagramOutput);
            fileStream.Write(res);
        }
        catch(InvalidOperationException ioe)
        {
            Console.WriteLine($"Image generator: {ioe.Message}");
        }
    }

    private static KrokiDiagramType GetDiagramType(ModelType sourceType) =>
        Enum.Parse<KrokiDiagramType>(sourceType.ToString(), true);
       
    private static KrokiOutputType GetOutputType(string outputType) => outputType.ToLower() switch
    {
        "base64" => KrokiOutputType.Base64,
        "jpg" => KrokiOutputType.JPG,
        "pdf" => KrokiOutputType.PDF,
        "png" => KrokiOutputType.PNG,
        "svg" => KrokiOutputType.SVG,
        "txt" => KrokiOutputType.TXT,
        _ => KrokiOutputType.None
    };

    private static string GetExtensionForOutputType(KrokiOutputType outputType) => outputType switch
    {
        KrokiOutputType.None => "txt",
        KrokiOutputType.PNG => "png",
        KrokiOutputType.SVG => "svg",
        KrokiOutputType.JPG => "jpg",
        KrokiOutputType.PDF => "pdf",
        KrokiOutputType.TXT => "txt",
        KrokiOutputType.Base64 => "b64"
    };

}
