using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Services;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;

namespace bad_each_way_finder.Pages
{
    [Authorize(Roles = "Admin, User")]
    public class RacesModel : PageModel
    {
        public List<Race> Races { get; set; }
        public Race SelectedRace { get; set; }

        private readonly IApiService _apiService;
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        public RacesModel(IApiService apiService, ILoginService loginService,
            ITokenService tokenService)
        {
            _apiService = apiService;
            _loginService = loginService;
            _tokenService = tokenService;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                await GetDto();

                var token = _tokenService.JwtToken;
                var tokenValidation = _tokenService.ValidateToken();

                if (!tokenValidation)
                {
                    await _loginService.Logout(token);
                    return Redirect("Identity/Account/Logout");
                }

                return Page();

            }
            catch (ApiServiceException ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.Message.Contains("Invalid Token"))
                {
                    return Redirect("Identity/Account/Logout");
                }

                TempData["Exception"] = JsonConvert.SerializeObject(
                    new Exception("Could not get GetRacesAndPropositionsDto()"));
                return RedirectToPage("./Error");
            }
        }
        public async Task<PartialViewResult> OnGetSelectedRace(string raceMeetingTime)
        {
            try
            {
                await GetDto();
                var splitVariable = raceMeetingTime.Split(" ~ ");
                var races = Races.Where(p => p.EventName == splitVariable[0]);
                var selectedRace = races.FirstOrDefault(p => p.EventDateTime.ToString("HH:mm") == splitVariable[1]);
                if (selectedRace == null)
                {
                    SelectedRace = new Race();
                }
                else
                {
                    SelectedRace = selectedRace;
                }
                return PartialView("SelectedRacePartial", this);
            }
            catch (ApiServiceException ex)
            {
                ModelState.AddModelError("Exception Message", $"{ex.Message}");
                Console.WriteLine(ex.Message);
                return PartialView("SelectedRacePartial", this);
            }
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
            try
            {
                var Dto = await _apiService.GetRacesAndPropositionsDto();

                Races = Dto?.Races ?? new List<Race>();
            }
            catch (ApiServiceException apiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
