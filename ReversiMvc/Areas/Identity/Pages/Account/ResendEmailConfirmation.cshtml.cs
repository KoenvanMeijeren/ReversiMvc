// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ReversiMvc.Security;

namespace ReversiMvc.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IRecaptcha _recaptcha;

    public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IRecaptcha recaptcha)
    {
        this._userManager = userManager;
        this._emailSender = emailSender;
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
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
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

        var user = await this._userManager.FindByEmailAsync(this.Input.Email);
        if (user == null)
        {
            this.ModelState.AddModelError(string.Empty, "Verificatie email verzonden. Controleer uw email.");
            return this.Page();
        }

        var userId = await this._userManager.GetUserIdAsync(user);
        var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = this.Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { userId = userId, code = code },
            protocol: this.Request.Scheme);
        await this._emailSender.SendEmailAsync(this.Input.Email,
            "Bevestig uw email",
            $"Bevestig uw account door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier te klikken</a>.");

        this.ModelState.AddModelError(string.Empty, "Verificatie email verzonden. Controleer uw email.");
        return this.Page();
    }
}
