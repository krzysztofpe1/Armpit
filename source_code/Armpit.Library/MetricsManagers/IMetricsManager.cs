using Armpit.Library.DataModels.Metrics;

namespace Armpit.Library.MetricsManagers;

public interface IMetricsManager<T> where T : IMetrics
{
    public abstract Task<T> GetMetricsAsync();

}
