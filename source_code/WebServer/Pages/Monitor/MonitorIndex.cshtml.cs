using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebServer.Pages;

[BindProperties]
public class MonitorModel : PageModel
{
    public MonitorModel()
    {
    }

    public IActionResult OnGet()
    {
        return Page();
    }
}
