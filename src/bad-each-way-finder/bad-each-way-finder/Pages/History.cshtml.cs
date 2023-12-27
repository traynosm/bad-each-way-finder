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
            //get username from Httpcontext
            var userName = HttpContext.User.Identity!.Name;

            //check for logged in user
            if(string.IsNullOrEmpty(userName))
            {
                //if not logged in return empty page
                return;
            }

            //get propositions of logged in user
            var accountPropositions = await _apiService.GetAccountPropositions(userName);
            
            //filter propositions to display up to yesterday as is 'History' page
            accountPropositions = accountPropositions?
                .Where(p => p.EventDateTime.Date < DateTime.Today)
                .ToList();

            //assigning Propositions. Returning empty list if accountPropositions is null
            Propositions = accountPropositions ?? new List<Proposition>();
        }
    }
}
