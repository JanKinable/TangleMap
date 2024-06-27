using CommandLine;
using System.Text.RegularExpressions;

namespace TangleMap;
public class Options
{

    [Option('s', "solution", Required = true, HelpText = "Full path to the solution file")]
    public string Solution { get; set; } = "";

    [Option('g', "git", Required = true, HelpText = "Full path to the .get folder of the repository")]
    public string Repository { get; set; } = "";

    [Option('o', "output", Required = true, HelpText = "Full path to the output location of the mermaid file")]
    public string Output { get; set; } = "";

    [Option("filter", Required = false, HelpText = "Regex expression for filtering out projects by name")]
    public string? Filter { get; set; } = null;

    [Option("plugin", Required = false, HelpText = "The name of the plugin ")]
    public string Plugin { get; set; } = "TangleMap.Mermaid";

    public bool IsFiltered(string projectName)
    {
        if (string.IsNullOrEmpty(Filter)) return false;

        //string pattern = @".UnitTests|Tests";
        var rg = new Regex(Filter);
        return rg.IsMatch(projectName);
    }
}
