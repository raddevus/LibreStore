using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace LibreStore.Controllers;

[ApiController]
[Route("[controller]")]
public class CyaController : Controller
{
    private readonly ILogger<CyaController> _logger;

    public CyaController(ILogger<CyaController> logger)
    {
        _logger = logger;
    }

    [HttpPost("SaveData")]
    public ActionResult SaveData([FromForm] String key, [FromForm] String data){
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"SaveCyaData",key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        SqliteCyaProvider scp = new SqliteCyaProvider();
        Cya c = new Cya(mainTokenId,data);
        CyaData cd = new CyaData(scp,c);
        cd.Configure();
        var cyaId = scp.Save();
    
        var jsonResult = new {success=true,CyaId=cyaId};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key){
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"GetCyaData",key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        
        SqliteCyaProvider scp = new SqliteCyaProvider();
        Cya c = new Cya(mainTokenId);
        CyaData cd = new CyaData(scp,c);
        cd.ConfigureSelect(mainTokenId);
        c = scp.GetCyaBucket();

        // if Bucket.Id is > 0 then a valid bucket was returned
        // otherwise there was not matching bucket (b.id == 0)
        var jsonResult = new {success=(c.Id > 0),cyabucket=c};
        return new JsonResult(jsonResult);
    }

    private Int64 WriteUsage(SqliteProvider sp, String action, String key="", bool shouldInsert=true){
        var ipAddress = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        
        MainTokenData mtd = new MainTokenData(sp,new MainToken(key));
        
        if (shouldInsert){
            mtd.ConfigureInsert();
        }
        else{
            mtd.ConfigureSelect();
        }
        var mainTokenId = sp.GetOrInsert();

        Usage u = new Usage(mainTokenId,ipAddress,action);
        UsageData ud = new UsageData(sp,u);
        ud.Configure();
        sp.Save();
        return mainTokenId;
    }

    public string Hash(string value) 
    { 
        var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(value)); 
        return String.Concat(Array.ConvertAll(hash, x => x.ToString("X2"))); 
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
   
}
