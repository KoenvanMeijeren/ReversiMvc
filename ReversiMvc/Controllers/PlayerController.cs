#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

[Authorize(policy: "canManagePlayer")]
public class PlayerController : Controller
{
    private readonly IPlayersRepository _repository;
    private readonly ILogger<PlayerController> _logger;
    private readonly string _currentUser;
    private readonly UserManager<IdentityUser> _userManager;

    public PlayerController(IPlayersRepository repository, ILogger<PlayerController> logger, ICurrentUserService currentUser, UserManager<IdentityUser> userManager)
    {
        this._repository = repository;
        this._logger = logger;
        this._currentUser = currentUser.Name;
        this._userManager = userManager;
    }

    // GET: Players
    public async Task<IActionResult> Index()
    {
        this._logger.LogInformation("User {User} has viewed all players", this._currentUser);

        return this.View(await this._repository.AllAsync());
    }

    // GET: Players/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var playerEntity = await this._repository.GetDbSet()
            .FirstOrDefaultAsync(m => m.Guid == id);
        if (playerEntity == null)
        {
            return this.NotFound();
        }

        this._logger.LogInformation("User {User} has viewed the details of player {Player}", this._currentUser, playerEntity.Guid);

        return this.View(playerEntity);
    }

    // GET: Players/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        var playerEntity = await this._repository.GetDbSet().FindAsync(id);
        if (playerEntity == null)
        {
            return this.NotFound();
        }
        return this.View(playerEntity);
    }

    // POST: Players/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Guid,Name,Victories,Losses,Draws")] PlayerEntity playerEntity)
    {
        if (id != playerEntity.Guid)
        {
            return this.NotFound();
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(playerEntity);
        }

        try
        {
            await this._repository.UpdateAsync(playerEntity);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.PlayerEntityExists(playerEntity.Guid))
            {
                return this.NotFound();
            }

            throw;
        }

        this._logger.LogInformation("User {User} has updated player {Player}", this._currentUser, playerEntity.Guid);

        return this.RedirectToAction(nameof(this.Index));
    }

    // GET: Players/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return this.NotFound();
        }

        if (id.Equals(this._currentUser))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je mag je eigen account niet verwijderen!" });
        }

        var playerEntity = await this._repository.GetDbSet()
            .FirstOrDefaultAsync(m => m.Guid == id);
        if (playerEntity == null)
        {
            return this.NotFound();
        }

        return this.View(playerEntity);
    }

    // POST: Players/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (id.Equals(this._currentUser))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je mag je eigen account niet verwijderen!" });
        }

        var playerEntity = await this._repository.GetDbSet().FindAsync(id);

        await this._repository.DeleteAsync(playerEntity);
        
        var user = await this._userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await this._userManager.DeleteAsync(user);
            
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }
        
            this._logger.LogInformation("User with ID '{id}' deleted themselves.", id);
        }

        this._logger.LogInformation("User {User} has deleted player {Player}", this._currentUser, playerEntity.Guid);

        return this.RedirectToAction(nameof(this.Index));
    }

    private bool PlayerEntityExists(string guid)
    {
        return this._repository.GetDbSet().Any(e => e.Guid == guid);
    }
}
