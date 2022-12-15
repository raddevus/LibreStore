using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;

namespace LibreStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    
    public IHostApplicationLifetime _lifeTime;

    public HomeController(ILogger<HomeController> logger, IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _lifeTime = appLifetime;
    }



    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("StopService")]
    public ActionResult StopService(String pwd){
        if (DataController.Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        _lifeTime.StopApplication();
        return new JsonResult(new {result="true",message="LibreStore is shutting down."});
    }
}
