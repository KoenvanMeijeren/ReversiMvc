﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReversiMvc.Security;

namespace ReversiMvc.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly IRecaptcha _recaptcha;

    public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IRecaptcha recaptcha)
    {
        this._signInManager = signInManager;
        this._logger = logger;
        this._recaptcha = recaptcha;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(this.ErrorMessage))
        {
            this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
        }

        returnUrl ??= this.Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        this.ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= this.Url.Content("~/");

        this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        var recaptchaPassed = await this._recaptcha.Validate(this.Request.Form, this.HttpContext.Connection.RemoteIpAddress);
        if (!recaptchaPassed)
        {
            this.ModelState.AddModelError(string.Empty, IRecaptcha.InvalidMessage);
            return this.Page();
        }

        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await this._signInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            this._logger.LogInformation("User logged in.");
            return this.LocalRedirect(returnUrl);
        }
        
        if (result.RequiresTwoFactor)
        {
            return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = this.Input.RememberMe });
        }
        
        if (result.IsLockedOut)
        {
            this._logger.LogWarning("User account locked out.");
            return this.RedirectToPage("./Lockout");
        }

        this.ModelState.AddModelError(string.Empty, "Ongeldige inlog poging.");
        return this.Page();
    }
}
