namespace Armpit.Library.DataModels.Metrics;

public class CpuMetrics : IMetrics
{
    public List<double> CoresUsage { get; set; }
    public double TotalUsage { get; set; }
    public string CpuModel { get; set; }
    public double BoostClockSpeed { get; set; }
    public double BaseClockSpeed { get; set; }
    public double Temperature { get; set; }
}
