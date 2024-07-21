using Armpit.Library.DataModels.Metrics;
using System.Diagnostics;

namespace Armpit.Library.MetricsManagers.Managers;

public class MemoryMetricsManager : MetricsManagerPlatformDivisionBase<MemoryMetrics>
{
    protected override async Task<MemoryMetrics> GetUnixMetricsAsync()
    {
        var output = "";

        var memoryInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"free -m\"",
            RedirectStandardOutput = true
        };

        using (var process = Process.Start(memoryInfo))
        {
            output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
        }

        var lines = output.Split("\n");
        var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var swap = lines[2].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics
        {
            Total = int.Parse(memory[1]),
            Used = int.Parse(memory[2]),
            Cached = int.Parse(memory[5]),
            SwapTotal = int.Parse(swap[1]),
            SwapUsed = int.Parse(swap[2])
        };

        return metrics;
    }

    protected override async Task<MemoryMetrics> GetWindowsMetricsAsync()
    {
        var output = await ExecuteWmicCommandAsync("OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
        var lines = output.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var freeMemory = int.Parse(lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim());
        var totalMemory = int.Parse(lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim());

        var swapOutput = await ExecuteWmicCommandAsync("PageFile get AllocatedBaseSize,CurrentUsage /Value");
        var swapLines = swapOutput.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var swapTotal = int.Parse(swapLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim());
        var swapUsed = int.Parse(swapLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim());

        var cachedOutput = await ExecuteWmicCommandAsync("OS get FreeSpaceInPagingFiles,SizeStoredInPagingFiles /Value");
        var cachedLines = cachedOutput.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var cachedMemory = int.Parse(cachedLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim());

        var metrics = new MemoryMetrics
        {
            Total = totalMemory/1024,
            Used = (totalMemory - freeMemory)/1024,
            Cached = cachedMemory/1024,
            SwapTotal = swapTotal,
            SwapUsed = swapUsed
        };

        return metrics;
    }

    private async Task<string> ExecuteWmicCommandAsync(string arguments)
    {
        var info = new ProcessStartInfo
        {
            FileName = "wmic",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(info))
        {
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output;
        }
    }
}
