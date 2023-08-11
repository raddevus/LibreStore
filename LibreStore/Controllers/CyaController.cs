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
    public ActionResult SaveData([FromForm] String key,
            [FromForm] String data,
            [FromForm] String hmac,
            [FromForm] String iv){
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("SaveCyaData",GetIpAddress(),key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        SqliteCyaProvider scp = new SqliteCyaProvider();
        Cya c = new Cya(mainTokenId,data,hmac,iv);
        CyaData cd = new CyaData(scp,c);
        cd.Configure();
        var cyaId = scp.Save();
    
        var jsonResult = new {success=true,CyaId=cyaId};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key){
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("GetCyaData",GetIpAddress(),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        
        ICyaDbProvider scp = new CyaDbProvider(DbType.Sqlite);
        Cya c = new Cya(mainTokenId);
       
        scp.ConfigureSelect(mainTokenId);
        c = scp.GetCyaBucket();

        // if Bucket.Id is > 0 then a valid bucket was returned
        // otherwise there was not matching bucket (b.id == 0)
        var jsonResult = new {success=(c.Id > 0),cyabucket=c};
        return new JsonResult(jsonResult);
    }

    [HttpGet("DeleteData")]
    public ActionResult DeleteData(String key)
    {
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("DeleteCyaData",GetIpAddress(),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Cya data because of invalid MainToken.Key. Data not deleted!"};
            return new JsonResult(jsonErrorResult);    
        }

        SqliteCyaProvider scp = new SqliteCyaProvider();
        Cya c = new Cya(mainTokenId);
        CyaData cd = new CyaData(scp,c);
        cd.ConfigureDelete(mainTokenId);
        var deletedCount = scp.DeleteCyaBucket();
        Object? jsonResult = null;
        if (deletedCount > -1){
            jsonResult = new {success=(deletedCount > -1),message="Encrypted data & all associated data has been deleted."};
        }
        else{
            jsonResult = new {success=(deletedCount > -1),message="Data does not exist for associated Cya Secret ID (MainToken). No data deleted."};
        }
        return new JsonResult(jsonResult);
    }

    private string GetIpAddress(){
        return Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
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
