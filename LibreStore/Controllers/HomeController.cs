using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;

namespace LibreStore.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _config;
    private IHostApplicationLifetime _lifeTime;
    private string dbType;

    public HomeController(IConfiguration _configuration,  IHostApplicationLifetime appLifetime){
        _config = _configuration;
        _lifeTime = appLifetime;
        dbType = _config["dbType"];
        Console.WriteLine($"###### {dbType} ##########");
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
        DbCommon dbc = new DbCommon(HelperTool.GetDbType(dbType));
        if (HelperTool.Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            dbc.WriteUsage("StopService - FAIL!", HelperTool.GetIpAddress(Request),"",false);    
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        
        dbc.WriteUsage("StopService", HelperTool.GetIpAddress(Request),"",false);
        _lifeTime.StopApplication();
        return new JsonResult(new {result="true",message="LibreStore is shutting down."});
    }

    [HttpGet("GetTime")]
    public ActionResult GetTime(){
        return new JsonResult(new {result="true",message=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
    }
}
