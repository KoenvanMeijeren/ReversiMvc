// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Principal;

namespace ReversiMvc.Services.Contracts;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Guid { get; }
    string? Name { get; }
    IIdentity? Identity { get; }
    
    bool HasRole(string compareRole);

    bool IsAdmin();
    bool IsMediator();
}
