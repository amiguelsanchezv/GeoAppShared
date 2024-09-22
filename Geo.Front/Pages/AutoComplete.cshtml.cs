using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Geo.Front.Pages
{
    public class AutoCompleteModel : PageModel
    {
        private readonly ILogger<AutoCompleteModel> _logger;

        public AutoCompleteModel(ILogger<AutoCompleteModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
