// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ReversiMvc.Models;

public class EditPasswordViewModel
{

    public IdentityUser User { get; }

    public string Guid { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Het nieuwe wachtwoord moet minimaal {2} en maximaal {1} karakters lang zijn.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Het bevestigingswachtwoord moet minimaal {2} en maximaal {1} karakters lang zijn.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Het nieuwe wachtwoord en het bevestigingswachtwoord komen niet overeen.")]
    public string ConfirmPassword { get; set; }

    public EditPasswordViewModel()
    {
        
    }

    public EditPasswordViewModel(IdentityUser user)
    {
        this.User = user;
        this.Guid = user.Id;
    }
    
}
