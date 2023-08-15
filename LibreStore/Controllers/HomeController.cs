using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;

namespace LibreStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    
    private IHostApplicationLifetime _lifeTime;

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
        
        // TODO: Read DB type from some type of app config
        IDataDbProvider dbp = new DataDbProvider(DbType.Sqlite);
        if (HelperTool.Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            dbp.WriteUsage("StopService - FAIL!", HelperTool.GetIpAddress(Request),"",false);    
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        
        dbp.WriteUsage("StopService", HelperTool.GetIpAddress(Request),"",false);
        _lifeTime.StopApplication();
        return new JsonResult(new {result="true",message="LibreStore is shutting down."});
    }

    [HttpGet("GetTime")]
    public ActionResult GetTime(){
        return new JsonResult(new {result="true",message=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
    }
}
