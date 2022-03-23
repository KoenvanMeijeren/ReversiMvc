// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Identity;

namespace ReversiMvc.Models;

public class UsersViewModel
{
    public IEnumerable<IdentityUser> Users { get; }

    public UsersViewModel(IEnumerable<IdentityUser> users)
    {
        this.Users = users;
    }

}
