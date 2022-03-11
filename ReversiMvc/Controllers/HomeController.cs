using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ReversiMvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPlayersRepository _repository;

    public HomeController(ILogger<HomeController> logger, IPlayersRepository repository)
    {
        this._logger = logger;
        this._repository = repository;
    }

    public IActionResult Index()
    {
        var user = this.User.Identity;
        var currentUserGuid = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserGuid != null && user != null)
        {
            this._repository.FirstOrCreate(new PlayerEntity {Guid = currentUserGuid, Name = user.Name});
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
