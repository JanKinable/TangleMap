using System.Text.RegularExpressions;

namespace TangleMap.Model;
public class Configuration
{
    public string Solution { get; set; } = "";

    public string Repository { get; set; } = "";

    public string Output { get; set; } = "";

    public string? Filter { get; set; } = null;

    public Dictionary<string, string> Parameters { get; set;} = new Dictionary<string, string>();
}
