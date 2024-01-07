using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace bad_each_way_finder.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class HistoryModel : PageModel
    {
        public List<Proposition> Propositions { get; set; }

        private readonly IApiService _apiService;
        public HistoryModel(IApiService apiService)
        {
            _apiService = apiService;

            Propositions = new List<Proposition>();
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                //get username from Httpcontext
                var userName = HttpContext.User.Identity!.Name;

                //check for logged in user
                if (string.IsNullOrEmpty(userName))
                {
                    //if not logged in return empty page
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                //get propositions of logged in user
                var accountPropositions = await _apiService.GetAccountPropositions(userName);

                //filter propositions to display up to yesterday as is 'History' page
                accountPropositions = accountPropositions?
                    .Where(p => p.EventDateTime.Date < DateTime.Today)
                    .ToList();

                //assigning Propositions. Returning empty list if accountPropositions is null
                Propositions = accountPropositions ?? new List<Proposition>();

                return Page();
            }
            catch (ApiServiceException ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Exception"] = JsonConvert.SerializeObject(
                    new Exception("Could not GetAccountPropositions()"));
                return RedirectToPage("./Error");
            }
        }
    }
}
