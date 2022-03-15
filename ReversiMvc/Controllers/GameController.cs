using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Models.DataTransferObject;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

public class GameController : Controller
{
    private readonly IGameRepository _repository;
    private readonly PlayerEntity _currentPlayer;

    public GameController(IGameRepository repository, ICurrentPlayerService currentPlayerService)
    {
        this._repository = repository;
        this._currentPlayer = currentPlayerService.Get();
    }

    // GET: Game
    public async Task<IActionResult> Index()
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        return this.View(await this._repository.AllAsync());
    }

    // GET: Game/Create
    public async Task<IActionResult> Create()
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        return this.View();
    }

    // POST: Game/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Description")] GameJsonDto gameJsonDto)
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(gameJsonDto);
        }

        var game = await this._repository.AddAsync(gameJsonDto);
        await this._repository.AddPlayerOneAsync(game.Token, this._currentPlayer.Guid, this._currentPlayer.Name);

        return this.RedirectToAction(nameof(this.Details));
    }
    
    // GET: Game/{token}/details
    public async Task<IActionResult> Details(string? token)
    {
        var gameEntity = await this._repository.Get(token);
        if (gameEntity == null)
        {
            return this.NotFound();
        }

        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null && !entity.Token.Equals(gameEntity.Token))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        return this.View(new GameEditViewModel(gameEntity));
    }

    // POST: Game/add/player-two
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPlayerTwo(string id)
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        await this._repository.AddPlayerTwoAsync(id, this._currentPlayer.Guid, this._currentPlayer.Name);

        return this.RedirectToAction(nameof(this.Details), new { token = id });
    }
    
}
