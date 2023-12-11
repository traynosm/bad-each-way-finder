using bad_each_way_finder.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IApiService _apiService;

        public IndexModel(ILogger<IndexModel> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task OnGet()
        {
            var Dto = await _apiService.Get();
        }
    }
}