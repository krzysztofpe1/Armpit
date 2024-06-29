
using Armpit.Library.DataModels.Metrics;

namespace Armpit.Library.MetricsManagers;

public abstract class MetricsManagerBase<T> : IMetricsManager<T> where T : IMetrics
{
    public abstract Task<T> GetMetricsAsync();
}
