using bad_each_way_finder.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Composition;

namespace bad_each_way_finder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILoginService _loginService;

        public IndexModel(ILogger<IndexModel> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;

        }

        public IActionResult OnGet()
        {
            if (_loginService.EnsureBackend().Result == false)
            {
                TempData["Exception"] = JsonConvert.SerializeObject(new Exception("Backend is not available!"));
                return RedirectToPage("./Error"); 
            }
            return Page();
        }
    }
}