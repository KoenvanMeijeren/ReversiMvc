#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReversiMvc.Controllers;

[Authorize]
public class PlayerController : Controller
{
    private readonly IPlayersRepository _repository;

    public PlayerController(IPlayersRepository repository)
    {
        this._repository = repository;
    }

    // GET: Players
    public async Task<IActionResult> Index()
    {
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
        return this.RedirectToAction(nameof(this.Index));
    }

    private bool PlayerEntityExists(string guid)
    {
        return this._repository.GetDbSet().Any(e => e.Guid == guid);
    }
}
