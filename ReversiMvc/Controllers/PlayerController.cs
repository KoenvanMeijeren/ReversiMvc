#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

[Authorize]
public class PlayerController : Controller
{
    private readonly IPlayersRepository _repository;
    private readonly ILogger<PlayerController> _logger;
    private readonly string _currentUser;

    public PlayerController(IPlayersRepository repository, ILogger<PlayerController> logger, ICurrentUserService currentUser)
    {
        this._repository = repository;
        this._logger = logger;
        this._currentUser = currentUser.Name;
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

    // GET: Players/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: Players/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Guid,Name,Victories,Losses,Draws")] PlayerEntity playerEntity)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(playerEntity);
        }

        await this._repository.AddAsync(playerEntity);

        this._logger.LogInformation("User {User} has created player {Player}", this._currentUser, playerEntity.Guid);

        return this.RedirectToAction(nameof(this.Index));
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
        var playerEntity = await this._repository.GetDbSet().FindAsync(id);

        await this._repository.DeleteAsync(playerEntity);

        this._logger.LogInformation("User {User} has deleted player {Player}", this._currentUser, playerEntity.Guid);

        return this.RedirectToAction(nameof(this.Index));
    }

    private bool PlayerEntityExists(string guid)
    {
        return this._repository.GetDbSet().Any(e => e.Guid == guid);
    }
}
