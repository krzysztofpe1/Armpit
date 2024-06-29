using Armpit.Library.DataModels.Metrics;
using System.Runtime.InteropServices;

namespace Armpit.Library.MetricsManagers;

public abstract class MetricsManagerPlatformDivisionBase<T> : MetricsManagerBase<T> where T : IMetrics
{
    public override async Task<T> GetMetricsAsync()
    {
        if (IsUnix())
        {
            return await GetUnixMetricsAsync();
        }

        return await GetWindowsMetricsAsync();
    }
    protected virtual bool IsUnix()
    {
        var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        return isUnix;
    }

    protected abstract Task<T> GetWindowsMetricsAsync();

    protected abstract Task<T> GetUnixMetricsAsync();
}
