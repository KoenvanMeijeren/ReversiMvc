using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ReversiMvc.Services.Contracts;

namespace ReversiMvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ICurrentPlayerService _currentPlayerService;
    private readonly IGameRepository _gameRepository;

    public HomeController(ILogger<HomeController> logger, ICurrentPlayerService currentPlayerService, IGameRepository gameRepository)
    {
        this._currentPlayerService = currentPlayerService;
        this._gameRepository = gameRepository;
    }

    public async Task<IActionResult> Index()
    {
        // Todo: find out how to do this when a sign in event occurs.
        var currentPlayer = this._currentPlayerService.Get();
        var entity = await this._gameRepository.GetByPlayerToken(currentPlayer.Guid);
        if (entity != null)
        {
            return this.RedirectToAction("Details", "Game", new { token = entity.Token });
        }

        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
