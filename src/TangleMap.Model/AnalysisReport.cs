namespace TangleMap.Model;

public class AnalysisReport
{
    public List<SuspiciousPackage> SuspiciousPackages { get; set; } = [];
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
