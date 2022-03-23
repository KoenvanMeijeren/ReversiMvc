// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Claims;
using System.Security.Principal;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal? _user;

    public string? UserId => this._user?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Guid => this._user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? Name => this._user?.Identity?.Name;

    public IIdentity? Identity => this._user?.Identity;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this._user = httpContextAccessor.HttpContext?.User;
    }

    public bool HasRole(string compareRole)
    {
        return this._user != null && this._user.IsInRole(compareRole);
    }

    public bool IsAdmin()
    {
        return this.HasRole(ApplicationRoleTypes.Admin);
    }

    public bool IsMediator()
    {
        return this.HasRole(ApplicationRoleTypes.Mediator) || this.IsAdmin();
    }
}
