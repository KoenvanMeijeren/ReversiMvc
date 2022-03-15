// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReversiMvc.Areas.Identity.Pages.Account.Manage;

public class ExternalLoginsModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public ExternalLoginsModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserStore<IdentityUser> userStore)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._userStore = userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<UserLoginInfo> CurrentLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> OtherLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool ShowRemoveButton { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        this.CurrentLogins = await this._userManager.GetLoginsAsync(user);
        this.OtherLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => this.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();

        string passwordHash = null;
        if (this._userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
        {
            passwordHash = await userPasswordStore.GetPasswordHashAsync(user, this.HttpContext.RequestAborted);
        }

        this.ShowRemoveButton = passwordHash != null || this.CurrentLogins.Count > 1;
        return this.Page();
    }

    public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        var result = await this._userManager.RemoveLoginAsync(user, loginProvider, providerKey);
        if (!result.Succeeded)
        {
            this.StatusMessage = "De externe login is niet verwijderd.";
            return this.RedirectToPage();
        }

        await this._signInManager.RefreshSignInAsync(user);
        this.StatusMessage = "De externe login is verwijderd.";
        return this.RedirectToPage();
    }

    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = this.Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
        var properties = this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, this._userManager.GetUserId(this.User));
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
        }

        var userId = await this._userManager.GetUserIdAsync(user);
        var info = await this._signInManager.GetExternalLoginInfoAsync(userId);
        if (info == null)
        {
            throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
        }

        var result = await this._userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            this.StatusMessage = "De externe login is niet toegevoegd. Externe logins kunnen slechts aan één account worden gekoppeld.";
            return this.RedirectToPage();
        }

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        this.StatusMessage = "De externe login is toegevoegd.";
        return this.RedirectToPage();
    }
}
