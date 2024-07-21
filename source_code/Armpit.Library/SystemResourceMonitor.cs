using Armpit.Library.DataModels.Metrics;
using Armpit.Library.MetricsManagers;
using Armpit.Library.MetricsManagers.Managers;
using System.Diagnostics;

namespace Armpit.Library;

public class SystemResourceMonitor
{

    #region Private vars

    private IMetricsManager<MemoryMetrics> _memoryManager;
    private IMetricsManager<CpuMetrics> _cpuManager;

    #endregion
    public SystemResourceMonitor()
    {
        _memoryManager = new MemoryMetricsManager();
    }

    #region Public Methods

    public async Task<CombinedMetrics> GetCombinedMetricsAsync()
    {
        return new CombinedMetrics
        {
            MemoryMetrics = await _memoryManager.GetMetricsAsync()
        };
    }

    #endregion

}
