// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ReversiMvc.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _sender;

    public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
    {
        this._userManager = userManager;
        this._sender = sender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool DisplayConfirmAccountLink { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string EmailConfirmationUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
    {
        if (email == null)
        {
            return this.RedirectToPage("/Index");
        }

        var user = await this._userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with email '{email}'.");
        }

        this.Email = email;
        // Once you add a real email sender, you should remove this code that lets you confirm the account
        this.DisplayConfirmAccountLink = true;
        if (this.DisplayConfirmAccountLink)
        {
            var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await this._userManager.ConfirmEmailAsync(user, code);
            this.StatusMessage = result.Succeeded ? "Uw account is nu geactiveerd." : "Fout tijdens uw email bevestigen.";
        }

        return this.Page();
    }
}
