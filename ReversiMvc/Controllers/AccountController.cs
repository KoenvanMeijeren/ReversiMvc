// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReversiMvc.Controllers;

[Authorize(policy: "canManageUsers")]
public class AccountController : Controller
{
    private readonly IRepository<IdentityUser> _repository;

    private readonly UserManager<IdentityUser> _userManager;
    
    public AccountController(IRepository<IdentityUser> usersRepository, UserManager<IdentityUser> userManager)
    {
        this._repository = usersRepository;
        this._userManager = userManager;
    }
    
    // GET
    public IActionResult Users()
    {
        return this.View(new UsersViewModel(this._repository.All()));
    }

    // GET
    public async Task<IActionResult> Roles(string guid)
    {
        var user = await this._userManager.FindByIdAsync(guid);
        if (user == null)
        {
            return this.NotFound("Deze gebruiker bestaat niet!");
        }
        
        var roles = await this._userManager.GetRolesAsync(user);

        return this.View(new ClaimEditViewModel { Guid = guid, Roles = roles });
    }
    
    // POST
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Roles(ClaimEditViewModel viewModel)
    {
        var user = await this._userManager.FindByIdAsync(viewModel.Guid);
        if (user == null)
        {
            return this.NotFound("Deze gebruiker bestaat niet!");
        }

        var roles = await this._userManager.GetRolesAsync(user);

        if (roles.Contains(viewModel.Role))
        {
            this.ModelState.AddModelError(string.Empty, "Deze rol is al toegevoegd aan de gebruiker!");
            return this.View(new ClaimEditViewModel {Guid = viewModel.Guid, Roles = roles});
        }
        
        var result = await this._userManager.AddToRoleAsync(user, viewModel.Role);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, err.Description);
            }
        }
        
        roles = await this._userManager.GetRolesAsync(user);
        
        return this.View(new ClaimEditViewModel {Guid = viewModel.Guid, Roles = roles});
    }
    
    // GET
    public async Task<IActionResult> DeleteRole(string guid, string role)
    {
        var user = await this._userManager.FindByIdAsync(guid);
        var roles = await this._userManager.GetRolesAsync(user);

        if (!roles.Contains(role))
        {
            return this.RedirectToAction(nameof(this.Roles), new { guid });
        }
        
        await this._userManager.RemoveFromRoleAsync(user, role);
        
        return this.RedirectToAction(nameof(this.Roles), new { guid });
    }

}
