// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using bad_each_way_finder.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bad_each_way_finder.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ITokenService _tokenService;
        private readonly IApiService _apiService;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger,
            ITokenService tokenService, IApiService apiService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
            _apiService = apiService;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            var token = _tokenService.JwtToken;
            await _apiService.Logoout(token);

            _tokenService.RevokeToken();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
