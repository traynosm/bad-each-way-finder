using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;
using bad_each_way_finder_domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace bad_each_way_finder.Pages.Shared
{
    [Authorize(Roles = "Admin, User")]
    public class PropositionsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly ILoginService _loginService;

        public List<Proposition> LivePropositions { get; set; }
        public List<Proposition> AccountPropositions { get; set; }
        public List<Proposition> RaisedPropositions { get; set; }
        public List<Proposition> NewlyRaisedPropositions { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public PropositionsModel(IApiService apiService, ITokenService tokenService,
            ILoginService loginService)
        {
            _apiService = apiService;
            _tokenService = tokenService;
            _loginService = loginService;

            LivePropositions = new List<Proposition>();
            AccountPropositions = new List<Proposition>();
            RaisedPropositions = new List<Proposition>();
            NewlyRaisedPropositions = new List<Proposition>();

            StatusMessage = string.Empty;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var userName = HttpContext.User.Identity!.Name;
                await GetDto(userName!);

                var token = _tokenService.JwtToken;
                var tokenValidation = _tokenService.ValidateToken();        

                if(!tokenValidation)
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
        public async Task<PartialViewResult> OnGetPropositions()
        {
            try
            {
                var userName = HttpContext.User.Identity!.Name;
                await GetDto(userName!);

                if (NewlyRaisedPropositions.Any())
                {
                    var msg = string.Empty;
                    foreach (var prop in NewlyRaisedPropositions)
                    {
                        msg += $"{prop.EventName} " +
                            $" {prop.EventDateTime.ToString("HH:mm")} -" +
                            $" {prop.RunnerName} " +
                            $" {prop.WinRunnerOddsDecimal.ToString("0.00")} " +
                            $" {prop.EachWayExpectedValue.ToString("0.00%")};";
                    }

                    var splitString = msg.Split(';');
                    var withoutEmpties = splitString.Where(m => !string.IsNullOrEmpty(m));
                    var statusMsg = String.Join("~", withoutEmpties);
                    StatusMessage = statusMsg;
                }

                return PartialView("PropositionsPartial", this);
            }
            catch (ApiServiceException ex)
            {
                ModelState.AddModelError("Exception Message", $"{ex.Message}");
                Console.WriteLine(ex.Message);
                return PartialView("PropositionsPartial", this);
            }
        }

        public async Task<PartialViewResult> OnPostAddAccountProposition()
        {
            try
            {
                var form = HttpContext.Request.Form;
                var runnerName = form["runner-name"].ToString();
                var winOdds = double.Parse(form["win-odds"].ToString());
                var eventId = form["event-id"].ToString();
                var user = HttpContext.User.Identity!.Name;

                var Dto = new RaisedPropositionDto()
                {
                    RunnerName = runnerName,
                    WinRunnerOddsDecimal = winOdds,
                    EventId = eventId,
                    IdentityUserName = user!,
                };

                var accountPropositions = await _apiService.PostRaisedPropostionDto(Dto);

                accountPropositions = accountPropositions?
                    .Where(p => p.EventDateTime.Date >= DateTime.Today)
                    .ToList();

                AccountPropositions = accountPropositions;

                return PartialView("AccountPropositionsPartial", this);
            }
            catch (ApiServiceException ex)
            {
                ModelState.AddModelError("Exception Message", $"{ex.Message}");
                Console.WriteLine(ex.Message);
                return PartialView("AccountPropositionsPartial", this);
            }
        }

        public async Task<PartialViewResult> OnPostRemoveAccountProposition()
        {
            try
            {
                var form = HttpContext.Request.Form;
                var runnerName = form["runner-name"].ToString();
                var winOdds = double.Parse(form["win-odds"].ToString());
                var eventId = form["event-id"].ToString();
                var user = HttpContext.User.Identity!.Name;

                var Dto = new RaisedPropositionDto()
                {
                    RunnerName = runnerName,
                    WinRunnerOddsDecimal = winOdds,
                    EventId = eventId,
                    IdentityUserName = user!,
                };

                var accountPropositions = await _apiService.RemoveRaisedPropostionDto(Dto);

                accountPropositions = accountPropositions?
                    .Where(p => p.EventDateTime.Date >= DateTime.Today)
                    .ToList();

                AccountPropositions = accountPropositions;

                ViewData["PropositionType"] = "Account";

                return PartialView("AccountPropositionsPartial", this);
            }
            catch (ApiServiceException ex)
            {
                ModelState.AddModelError("Exception Message", $"{ex.Message}");
                Console.WriteLine(ex.Message);
                return PartialView("AccountPropositionsPartial", this);
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

        public async Task GetDto(string userName)
        {
            try
            {
                var Dto = await _apiService.GetRacesAndPropositionsDto();

                LivePropositions = Dto!.LivePropositions;
                RaisedPropositions = Dto!.RaisedPropositions;
                NewlyRaisedPropositions = Dto!.NewlyRaisedPropositions;

                var accountPropositions = await _apiService.GetAccountPropositions(userName);

                accountPropositions = accountPropositions?
                    .Where(p => p.EventDateTime.Date >= DateTime.Today)
                    .ToList();

                AccountPropositions = accountPropositions ?? new List<Proposition>();
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
