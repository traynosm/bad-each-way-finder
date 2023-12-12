using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Pages
{
    public class RacesModel : PageModel
    {
        public List<Proposition> Propositions { get; set; }
        public List<Race> Races { get; set; }

        private readonly IApiService _apiService;

        public RacesModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task OnGet()
        {
            var Dto = await _apiService.Get();
            Propositions = Dto.Propositions;
            Races = Dto.Races;
        }
    }
}
