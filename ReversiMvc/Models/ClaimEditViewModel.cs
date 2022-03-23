﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReversiMvc.Models;

public class ClaimEditViewModel
{

    public string Guid { get; set; }

    public IEnumerable<string> Roles { get; set; }

    public SelectList roleSelectList => new SelectList(ApplicationRoleTypes.All);

    public string Role { get; set; }

}
