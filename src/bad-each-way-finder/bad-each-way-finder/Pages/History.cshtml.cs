using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Pages
{
    public class HistoryModel : PageModel
    {
        public List<Proposition> Propositions { get; set; }

        private readonly IApiService _apiService;
        public HistoryModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task OnGet()
        {
            var userName = HttpContext.User.Identity!.Name;

            var accountPropositions = await _apiService.GetAccountPropositions(userName);
            Propositions = accountPropositions ?? new List<Proposition>();
        }
    }
}
