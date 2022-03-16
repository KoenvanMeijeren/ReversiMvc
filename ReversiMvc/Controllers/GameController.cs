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

        var entities = from game in await this._repository.AllAsync() select new GameEditViewModel(game, this._currentPlayer);
        
        return this.View(entities);
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

        return this.RedirectToAction(nameof(this.Details), new { token = game.Token });
    }
    
    // GET: Game/{token}/details
    public async Task<IActionResult> Details(string? token)
    {
        var gameEntity = await this._repository.Get(token);
        if (gameEntity == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null && !entity.Token.Equals(gameEntity.Token))
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        return this.View(new GameEditViewModel(gameEntity, this._currentPlayer));
    }
    
    public async Task<IActionResult> AddPlayerTwo(string? token)
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }
        
        return this.View(new GameEditViewModel(game, this._currentPlayer));
    }

    // POST: Game/add/player-two
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("AddPlayerTwo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPlayerTwoConfirmed(string? token)
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        await this._repository.AddPlayerTwoAsync(game.Token, this._currentPlayer.Guid, this._currentPlayer.Name);

        return this.RedirectToAction(nameof(this.Details), new { token });
    }
    
    public async Task<IActionResult> Start(string? token)
    {
        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }
        
        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanStart())
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Alleen speler 1 kan het spel starten!" });
        }
        
        return this.View(new GameEditViewModel(game, this._currentPlayer));
    }

    // POST: Game/add/player-two
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("Start")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartConfirmed(string? token)
    {
        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanStart())
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Alleen speler 1 kan het spel starten!" });
        }

        await this._repository.StartAsync(game.Token);

        return this.RedirectToAction(nameof(this.Details), new { token });
    }
    
    public async Task<IActionResult> Quit(string? token)
    {
        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }
        
        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanQuit())
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je kan alleen het spel stoppen waar jij aan gekoppeld bent!" });
        }
        
        return this.View(new GameEditViewModel(game, this._currentPlayer));
    }

    // POST: Game/add/player-two
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("Quit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitConfirmed(string? token)
    {
        var game = await this._repository.Get(token);
        if (game == null)
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }
        
        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanQuit())
        {
            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je kan alleen het spel stoppen waar jij aan gekoppeld bent!" });
        }

        await this._repository.QuitAsync(game.Token);

        return this.RedirectToAction(nameof(this.Details), new { token });
    }
    
}
