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

    [Option('t', "outputType", Required = true, HelpText = "The output format (see options https://kroki.io/#support)")]
    public string OutputType { get; set; } = "";

    [Option("filter", Required = false, HelpText = "Regex expression for filtering out projects by name")]
    public string? Filter { get; set; } = null;

    [Option("includePackages", Required = false, HelpText = "Whether to include the package dependencies")]
    public bool IncludePackages { get; set; }


    public bool IsFiltered(string projectName)
    {
        if (string.IsNullOrEmpty(Filter)) return false;

        //string pattern = @".UnitTests|Tests";
        var rg = new Regex(Filter);
        return rg.IsMatch(projectName);
    }
}
