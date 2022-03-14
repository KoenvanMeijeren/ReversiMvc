// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Principal;

namespace ReversiMvc.Services.Contracts;

public interface ICurrentUserService
{
    public string? UserId { get; }
    public string? Guid { get; }
    public IIdentity? Identity { get; }
}
