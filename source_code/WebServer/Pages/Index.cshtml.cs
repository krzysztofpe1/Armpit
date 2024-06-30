using Armpit.Library.DataModels.Metrics;
using Armpit.Library.MetricsManagers.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebServer.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MemoryMetricsManager _metricsManager;

        public MemoryMetrics Metrics { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, MemoryMetricsManager memoryMetricsManager)
        {
            _logger = logger;
            _metricsManager = memoryMetricsManager;
        }

        public async Task<IActionResult> OnGet()
        {
            Metrics = await _metricsManager.GetMetricsAsync();
            return Page();
        }
    }
}
