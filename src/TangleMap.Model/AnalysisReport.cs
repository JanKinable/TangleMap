using System.Text;

namespace TangleMap.Model;

public class AnalysisReport
{
    public List<SuspiciousPackage> SuspiciousPackages { get; set; } = [];

    public string ToReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Suspecious packages:");
        foreach(var sp in SuspiciousPackages)
        {
            sb.AppendLine($"- {sp.PackageName} - {sp.Reason}");
            foreach(var af in sp.AdditionalInformation)
            {
                sb.AppendLine($"  * {af}");
            }
        }
        return sb.ToString();
    }
}

public class SuspiciousPackage
{
    public required string PackageName { get; set; }
    public SuspiciousPackageReason Reason { get; set; }

    public List<string> AdditionalInformation { get; set; } = [];
    
}

public enum SuspiciousPackageReason
{
    MultipleVersion
}
