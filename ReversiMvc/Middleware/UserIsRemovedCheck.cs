// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Identity;

namespace ReversiMvc.Middleware;

/// <summary>
/// Ensures that a removed user cannot access the application anymore.
/// </summary>
public class UserIsRemovedCheck
{
    private readonly RequestDelegate _next;

    public UserIsRemovedCheck(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var signInManager = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();

        var user = await userManager.GetUserAsync(context.User);
        if (user == null)
        {
            await signInManager.SignOutAsync();
        }

        await this._next.Invoke(context);
    }
}
