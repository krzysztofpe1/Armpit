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

    public float GetCurrentCpuUsage()
    {
        return 0;
    }



    #endregion

}
