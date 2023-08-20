using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Intrinsics.Arm;

namespace LibreStore.Controllers;

[ApiController]
[Route("[controller]")]
public class CyaController : Controller
{
    private readonly IConfiguration _config;
    private string dbType;

    public CyaController(IConfiguration _configuration){
        _config = _configuration;
        dbType = _config["dbType"];
        Console.WriteLine($"###### {dbType} ##########");
    }

    [HttpPost("SaveData")]
    public ActionResult SaveData([FromForm] String key,
            [FromForm] String data,
            [FromForm] String hmac,
            [FromForm] String iv){
        DbCommon dbc = new DbCommon(HelperTool.GetDbType(dbType));
        var mainTokenId = dbc.WriteUsage("SaveCyaData",HelperTool.GetIpAddress(Request),key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        ICyaDbProvider dbp = new CyaDbProvider(HelperTool.GetDbType(dbType));
        Cya c = new Cya(mainTokenId,data,hmac,iv);
        dbp.Configure(c);
        var cyaId = dbc.Save(dbp.DbConnection,dbp.DbCommand);
    
        var jsonResult = new {success=true,CyaId=cyaId};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key){
        DbCommon dbc = new DbCommon(HelperTool.GetDbType(dbType));
        var mainTokenId = dbc.WriteUsage("GetCyaData",HelperTool.GetIpAddress(Request),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Cya data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        
        ICyaDbProvider dbp = new CyaDbProvider(HelperTool.GetDbType(dbType));
        Cya c = new Cya(mainTokenId);
       
        dbp.ConfigureSelect(mainTokenId);
        c = dbp.GetCyaBucket();

        // if Bucket.Id is > 0 then a valid bucket was returned
        // otherwise there was not matching bucket (b.id == 0)
        var jsonResult = new {success=(c.Id > 0),cyabucket=c};
        return new JsonResult(jsonResult);
    }

    [HttpGet("DeleteData")]
    public ActionResult DeleteData(String key)
    {
        DbCommon dbc = new DbCommon(HelperTool.GetDbType(dbType));
        var mainTokenId = dbc.WriteUsage("DeleteCyaData",HelperTool.GetIpAddress(Request),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Cya data because of invalid MainToken.Key. Data not deleted!"};
            return new JsonResult(jsonErrorResult);    
        }

        ICyaDbProvider dbp = new CyaDbProvider(HelperTool.GetDbType(dbType));
        Cya c = new Cya(mainTokenId);

        dbp.ConfigureDelete(mainTokenId);
        var deletedCount = dbc.DeleteBucket(dbp.DbConnection,dbp.DbCommand);
        Object? jsonResult = null;
        if (deletedCount > 0){
            jsonResult = new {success=(deletedCount > 0),message="Encrypted data & all associated data has been deleted."};
        }
        else{
            jsonResult = new {success=(deletedCount > 0),message="Data does not exist for associated Cya Secret ID (MainToken). No data deleted."};
        }
        return new JsonResult(jsonResult);
    }
}
