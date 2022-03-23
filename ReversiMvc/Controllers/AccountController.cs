// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

[Authorize(policy: "canManageUsers")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    
    private readonly IRepository<IdentityUser> _repository;
    private readonly IPlayersRepository _playersRepository;
    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly string _currentUser;

    public AccountController(ILogger<AccountController> logger, IRepository<IdentityUser> usersRepository, UserManager<IdentityUser> userManager, ICurrentUserService currentUser, IPlayersRepository playersRepository)
    {
        this._logger = logger;
        this._repository = usersRepository;
        this._userManager = userManager;
        this._currentUser = currentUser.Guid;
        this._playersRepository = playersRepository;
    }

    // GET
    public IActionResult Index()
    {
        this._logger.LogInformation("User {User} has viewed all accounts", this._currentUser);
        
        return this.View(new UsersViewModel(this._repository.All()));
    }
    
    // GET: Players/Delete/5
    public async Task<IActionResult> Delete(string? guid)
    {
        if (guid == null)
        {
            return this.NotFound();
        }

        if (guid.Equals(this._currentUser))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je mag je eigen account niet verwijderen!" });
        }

        var user = await this._userManager.FindByIdAsync(guid);
        if (user == null)
        {
            return this.NotFound();
        }

        return this.View(user);
    }

    // POST: Players/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (id.Equals(this._currentUser))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je mag je eigen account niet verwijderen!" });
        }

        var user = await this._userManager.FindByIdAsync(id);
        if (user == null)
        {
            return this.NotFound();
        }

        await this._userManager.DeleteAsync(user);
        
        var player = this._playersRepository.Get(id);
        if (player != null)
        {
            await this._playersRepository.DeleteAsync(player);

            this._logger.LogInformation("Player with ID '{Id}' deleted", id);
        }

        this._logger.LogInformation("User {User} and player {Player} has been deleted", this._currentUser, id);

        return this.RedirectToAction(nameof(this.Index));
    }

    // GET
    public async Task<IActionResult> Roles(string guid)
    {
        this._logger.LogInformation("User {User} has viewed all accounts", this._currentUser);
        
        var user = await this._userManager.FindByIdAsync(guid);
        if (user == null)
        {
            this._logger.LogInformation("User {User} has viewed a non-existing account {Guid}", this._currentUser, guid);
            
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
        this._logger.LogInformation("User {User} has viewed all accounts", this._currentUser);
        
        var user = await this._userManager.FindByIdAsync(viewModel.Guid);
        if (user == null)
        {
            this._logger.LogInformation("User {User} has viewed a non-existing account {Guid}", this._currentUser, viewModel.Guid);
            
            return this.NotFound("Deze gebruiker bestaat niet!");
        }

        var roles = await this._userManager.GetRolesAsync(user);

        if (roles.Contains(viewModel.Role))
        {
            this._logger.LogInformation("User {User} has tried to add an already existing role to account {Guid}", this._currentUser, viewModel.Guid);
            
            this.ModelState.AddModelError(string.Empty, "Deze rol is al toegevoegd aan de gebruiker!");
            return this.View(new ClaimEditViewModel { Guid = viewModel.Guid, Roles = roles });
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

        return this.View(new ClaimEditViewModel { Guid = viewModel.Guid, Roles = roles });
    }

    // GET
    public async Task<IActionResult> DeleteRole(string guid, string role)
    {
        var user = await this._userManager.FindByIdAsync(guid);
        var roles = await this._userManager.GetRolesAsync(user);

        if (!roles.Contains(role))
        {
            this._logger.LogInformation("User {User} has tried to remove a non-existing role to account {Guid}", this._currentUser, guid);
            
            return this.RedirectToAction(nameof(this.Roles), new { guid });
        }

        this._logger.LogInformation("User {User} has removed role {Role} to account {Guid}", this._currentUser, role, guid);
        
        await this._userManager.RemoveFromRoleAsync(user, role);

        return this.RedirectToAction(nameof(this.Roles), new { guid });
    }

}
