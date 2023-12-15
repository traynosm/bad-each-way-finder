using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Pages.Shared
{
    public class PropositionsModel : PageModel
    {
        public List<Proposition> Propositions { get; set; }

        private readonly IApiService _apiService;

        public PropositionsModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task OnGet()
        {
            await GetDto();
        }
        public async Task<PartialViewResult> OnGetPropositions()
        {
            await GetDto();

            return PartialView("PropositionsPartial", this);
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

        public async Task GetDto()
        {
            var Dto = await _apiService.Get();
            Propositions = Dto.Propositions;
        }
    }
}
