using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Pages.Shared
{
    public class PropositionsModel : PageModel
    {
        private readonly IApiService _apiService;

        public List<Proposition> LivePropositions { get; set; }
        public List<Proposition> AccountPropositions { get; set; }
        public List<Proposition> SavedPropositions { get; set; }

        public PropositionsModel(IApiService apiService)
        {
            _apiService = apiService;

            LivePropositions = new List<Proposition>();
            AccountPropositions = new List<Proposition>();
            SavedPropositions = new List<Proposition>();
        }
        public async Task OnGet()
        {
            var userName = HttpContext.User.Identity!.Name;
            await GetDto(userName!);
        }
        public async Task<PartialViewResult> OnGetPropositions()
        {
            var userName = HttpContext.User.Identity!.Name;
            await GetDto(userName!);

            return PartialView("PropositionsPartial", this);
        }

        public async Task<PartialViewResult> OnPostAddAccountProposition()
        {
            var form = HttpContext.Request.Form;
            var runnerName = form["runner-name"].ToString();
            var winOdds = double.Parse(form["win-odds"].ToString());
            var eventId = form["event-id"].ToString();
            var user = HttpContext.User.Identity!.Name;

            var Dto = new SavedPropositionDto()
            {
                RunnerName = runnerName,
                WinRunnerOddsDecimal = winOdds,
                EventId = eventId,
                IdentityUserName = user!,
            };

            var accountPropositions = await _apiService.PostSavedPropostionDto(Dto);

            AccountPropositions = accountPropositions;

            return PartialView("AccountPropositionsPartial", this);
        }

        public async Task<PartialViewResult> OnPostRemoveAccountProposition()
        {
            var form = HttpContext.Request.Form;
            var runnerName = form["runner-name"].ToString();
            var winOdds = double.Parse(form["win-odds"].ToString());
            var eventId = form["event-id"].ToString();
            var user = HttpContext.User.Identity!.Name;

            var Dto = new SavedPropositionDto()
            {
                RunnerName = runnerName,
                WinRunnerOddsDecimal = winOdds,
                EventId = eventId,
                IdentityUserName = user!,
            };

            var accountPropositions = await _apiService.RemoveSavedPropostionDto(Dto);

            AccountPropositions = accountPropositions;

            ViewData["PropositionType"] = "Account";

            return PartialView("AccountPropositionsPartial", this);
        }

        [NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model)
        {
            ViewData.Model = model;

            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        public async Task GetDto(string userName)
        {
            var Dto = await _apiService.GetRacesAndPropositionsDto();
            LivePropositions = Dto!.LivePropositions;
            SavedPropositions = Dto!.SavedPropositions; 

            var accountPropositions = await _apiService.GetAccountPropositions(userName);
            AccountPropositions = accountPropositions ?? new List<Proposition>();
        }

    }
}
