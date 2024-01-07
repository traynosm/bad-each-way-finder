// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using bad_each_way_finder.Interfaces;
using bad_each_way_finder_domain.Exceptions;
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
        private readonly ILoginService _loginService;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger,
            ITokenService tokenService, ILoginService loginService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
            _loginService = loginService;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
                var token = _tokenService.JwtToken;
                await _loginService.Logout(token);
                _tokenService.RevokeToken();
            }
            catch (LoginServiceException ex)
            {
                Console.WriteLine($"Exception raised logging out of backend: {ex.Message}.");
            }
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
