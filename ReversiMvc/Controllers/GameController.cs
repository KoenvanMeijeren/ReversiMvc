using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReversiMvc.Models.DataTransferObject;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly IGameRepository _repository;
    private readonly PlayerEntity _currentPlayer;
    private readonly ILogger<GameController> _logger;
    private readonly string _currentUser;

    public GameController(IGameRepository repository, ICurrentPlayerService currentPlayerService, ILogger<GameController> logger)
    {
        this._repository = repository;
        this._currentPlayer = currentPlayerService.Get();
        this._logger = logger;
        this._currentUser = this._currentPlayer.Guid;
    }

    // GET: Game
    public async Task<IActionResult> Index()
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            this._logger.LogWarning("Player {Player} has tried to view all games while playing another game {Game}", this._currentUser, entity.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        this._logger.LogInformation("Player {User} has viewed all games", this._currentUser);

        var games = await this._repository.AllAsync();
        if (games == null)
        {
            return this.View();
        }

        var entities = from game in games select new GameEditViewModel(game, this._currentPlayer);

        return this.View(new GameOverviewViewModel(entities, this._currentPlayer));
    }

    // GET: Game/Create
    public async Task<IActionResult> Create()
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity == null)
        {
            return this.View();
        }

        this._logger.LogWarning("Player {Player} has tried to create a new game while playing the game {Game}", this._currentUser, entity.Token);

        return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });

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
            this._logger.LogWarning("Player {Player} has tried to create a new game while playing the game {Game}", this._currentUser, entity.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        if (!this.ModelState.IsValid)
        {
            return this.View(gameJsonDto);
        }

        var game = await this._repository.AddAsync(gameJsonDto);
        await this._repository.AddPlayerOneAsync(game.Token, this._currentPlayer.Guid, this._currentPlayer.Name);


        this._logger.LogInformation("Player {User} has created a new game {Game}", this._currentUser, game.Token);

        return this.RedirectToAction(nameof(this.Details), new { token = game.Token });
    }

    // GET: Game/{token}/details
    public async Task<IActionResult> Details(string? token)
    {
        var gameEntity = await this._repository.Get(token);
        if (gameEntity == null)
        {
            this._logger.LogWarning("Player {Player} has tried to view the details of the game {Game} while playing another game", this._currentUser, gameEntity.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null && !entity.Token.Equals(gameEntity.Token))
        {
            this._logger.LogWarning("Player {Player} has tried to view the details of a game while playing another game", this._currentUser);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        this._logger.LogInformation("Player {User} has viewed the details of game {Game}", this._currentUser, gameEntity.Token);

        return this.View(new GameEditViewModel(gameEntity, this._currentPlayer));
    }

    public async Task<IActionResult> AddPlayerTwo(string? token)
    {
        var entity = await this._repository.GetByPlayerToken(this._currentPlayer.Guid);
        if (entity != null)
        {
            this._logger.LogWarning("Player {Player} has tried to participate to the game {Game} while playing another game", this._currentUser, entity.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        var game = await this._repository.Get(token);
        if (game == null)
        {
            this._logger.LogWarning("Player {Player} has tried to participate to an unknown game", this._currentUser);

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
            this._logger.LogWarning("Player {Player} has tried to participate to the game {Game} while playing another game", this._currentUser, entity.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je bent al gekoppeld aan een spel!" });
        }

        var game = await this._repository.Get(token);
        if (game == null)
        {
            this._logger.LogWarning("Player {Player} has tried to participate to an unknown game", this._currentUser);

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
            this._logger.LogWarning("Player {Player} has tried to start an unknown game", this._currentUser);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanStart())
        {
            this._logger.LogWarning("Player {Player} has tried to start the game {Game} while playing another game", this._currentUser, viewModel.Token);

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
            this._logger.LogWarning("Player {Player} has tried to start an unknown game", this._currentUser);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanStart())
        {
            this._logger.LogWarning("Player {Player} has tried to start the game {Game} while playing another game", this._currentUser, viewModel.Token);

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
            this._logger.LogWarning("Player {Player} has tried to quit an unknown game", this._currentUser);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanQuit())
        {
            this._logger.LogWarning("Player {Player} has tried to quit the game {Game} while playing another game", this._currentUser, viewModel.Token);

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
            this._logger.LogWarning("Player {Player} has tried to quit an unknown game", this._currentUser);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Het gekozen spel bestaat niet!" });
        }

        var viewModel = new GameEditViewModel(game, this._currentPlayer);
        if (!viewModel.CanQuit())
        {
            this._logger.LogWarning("Player {Player} has tried to quit the game {Game} while playing another game", this._currentUser, viewModel.Token);

            return this.View("InvalidActionMessage", new InvalidActionViewModel { Message = "Je kan alleen het spel stoppen waar jij aan gekoppeld bent!" });
        }

        await this._repository.QuitAsync(game.Token);

        return this.RedirectToAction(nameof(this.Details), new { token });
    }

}
