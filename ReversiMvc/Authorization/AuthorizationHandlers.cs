// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using ReversiMvc.Authorization.Contracts;

namespace ReversiMvc.Authorization;

public abstract class AuthorizationHandlerBase<T> : AuthorizationHandler<T> where T : IAuthorizationRequirement
{
    protected bool CheckIfUserHasRole(AuthorizationHandlerContext context, string compareRole)
    {
        return context.User.IsInRole(compareRole);
    }
}

public class IsAdminHandler : AuthorizationHandlerBase<IAdminAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAdminAuthorizationRequirement requirement)
    {
        if (this.CheckIfUserHasRole(context, ApplicationRoleTypes.Admin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

}

public class IsMediatorHandler : AuthorizationHandlerBase<IMediatorAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IMediatorAuthorizationRequirement requirement)
    {
        if (this.CheckIfUserHasRole(context, ApplicationRoleTypes.Mediator)
            || this.CheckIfUserHasRole(context, ApplicationRoleTypes.Admin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

}
