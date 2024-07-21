using Armpit.Library;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.ApiControlers;

[ApiController]
[Route("api/[controller]")]
public class MonitorController : Controller
{

    private SystemResourceMonitor _systemResourceMonitor;

    public MonitorController(SystemResourceMonitor systemResourceMonitor)
    {
        _systemResourceMonitor = systemResourceMonitor;
    }

    [HttpGet]
    public async Task<IActionResult> GetSystemResourceMetrics()
    {
        var metrics = await _systemResourceMonitor.GetCombinedMetricsAsync();
        var memoryMetrics = metrics.MemoryMetrics;
        return new JsonResult(new
        {
            cpuMetrics = new { },
            memoryMetrics = new { Total = memoryMetrics.Total, Used = memoryMetrics.Used, Cached = memoryMetrics.Cached, SwapTotal = memoryMetrics.SwapTotal, SwapUsed = memoryMetrics.SwapUsed }
        });
    }

}
