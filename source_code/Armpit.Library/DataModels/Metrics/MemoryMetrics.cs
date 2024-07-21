namespace Armpit.Library.DataModels.Metrics;

/// <summary>
/// Every metric associated with memory size is in MB
/// </summary>
public class MemoryMetrics : IMetrics
{
    public int Total { get; set; }
    public int Used { get; set; }
    public int Cached { get; set; }
    public int SwapTotal { get; set; }
    public int SwapUsed { get; set; }
}
