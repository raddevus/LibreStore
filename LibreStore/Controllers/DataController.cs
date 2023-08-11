using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace LibreStore.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : Controller
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet("SaveData")]
    public ActionResult SaveData(String key, String data, String hmac, String iv, String? intent = null)
    {
        return InternalSaveData(key, data,hmac,iv,intent);
    }

    [HttpPost("SaveData")]
    public ActionResult SaveData([FromForm] String key,
        [FromForm] String data,
        [FromForm] String hmac,
        [FromForm] String iv,
        [FromForm] String? intent = null,
    bool dataIsPosted=true)
    {
        return InternalSaveData(key, data,hmac,iv,intent);
    }

    private ActionResult InternalSaveData(String key, String data, String hmac, String iv, String? intent = null){
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("SaveData",GetIpAddress(),key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        dbp = new DbProvider(DbType.Sqlite);
        Bucket b = new Bucket(mainTokenId,intent,data,hmac,iv);
        dbp.ConfigureBucket(b);
        
        var bucketId = dbp.Save();
    
        var jsonResult = new {success=true,BucketId=bucketId};
        return new JsonResult(jsonResult);
    }

    private string GetIpAddress(){
        return Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key, Int64 bucketId){
        
        IDbProvider dbProvider = new DbProvider(DbType.Sqlite);
        // When we call WriteUsage for GetData, we don't want to create a new MainToken
        // if it already doesn't exist, so we make last param = false
        dbProvider.WriteUsage("GetData", GetIpAddress(),key,false);

        dbProvider = new DbProvider(DbType.Sqlite);
        dbProvider.ConfigureBucketSelect(key, bucketId);
        // Bucket b = new Bucket(bucketId,mainTokenId);
        Bucket b = dbProvider.GetBucket();

        // if Bucket.Id is > 0 then a valid bucket was returned
        // otherwise there was not matching bucket (b.id == 0)
        var jsonResult = new {success=(b.Id > 0),bucket=b};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetBucketIds")]
    public ActionResult GetBucketIds(String key){
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("GetBucketIds",GetIpAddress(), key,false);

        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve any data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        dbp = new DbProvider(DbType.Sqlite);
        dbp.ConfigureBucketIdSelect(mainTokenId);
        List<long> allBucketIds = dbp.GetAllBucketIds();
        if (allBucketIds.Count() == 0){
            return new JsonResult(new {success="false",message="No data available for that key."});
        }
        return new JsonResult(new {bucketIds=allBucketIds});
    }

    [HttpGet("GetAllTokens")]
    public ActionResult GetAllTokens(String pwd){
        List<MainToken> allTokens = new List<MainToken>();
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        if (Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            dbp.WriteUsage("GetAllTokens - rejected",GetIpAddress(),"",false);
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        dbp.WriteUsage("GetAllTokens",GetIpAddress(),"",false);
        dbp = new DbProvider(DbType.Sqlite);
        allTokens = dbp.GetAllTokens();

        return new JsonResult(allTokens);
    }

    [HttpGet("DeleteData")]
    public ActionResult DeleteData(String key, long bucketId)
    {
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("DeleteData",GetIpAddress(),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't delete Data because of invalid MainToken.Key. Data not deleted!"};
            return new JsonResult(jsonErrorResult);
        }

        dbp = new DbProvider(DbType.Sqlite);
        dbp.ConfigureBucketDelete(bucketId, mainTokenId);
        var deletedCount = dbp.DeleteBucket();
        Object? jsonResult = null;
        if (deletedCount > 0){
            jsonResult = new {success=(deletedCount > -1),message="(Encrypted) data & all associated data has been deleted."};
        }
        else{
            jsonResult = new {success=(deletedCount > -1),message="Data does not exist for associated MainToken. No data deleted."};
        }
        return new JsonResult(jsonResult);
    }

    [HttpGet("AddOwner")]
    public ActionResult AddOwner(String key, String email){
        IDbProvider dbp = new DbProvider(DbType.Sqlite);
        var mainTokenId = dbp.WriteUsage("UpdateOwner",GetIpAddress(),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't add an owner because of invalid MainToken.Key. Please make sure you're using a valid MainToken Key!"};
            return new JsonResult(jsonErrorResult);
        }
        dbp = new DbProvider(DbType.Sqlite);
        Owner o = new Owner(email);
        dbp.ConfigureOwnerInsert(o.Email);
        o.ID = dbp.Save();
        
        
        dbp.ConfigureUpdateOwner(new MainToken(key,o.ID));
        Object? jsonResult = null;
         if (dbp.UpdateOwner() == 0){
            jsonResult = new {success=false,message="Couldn't update the Owner for that MainToken Key. Please try again."};            
         }
         else{
            jsonResult = new {success=true,message="The Owner has been set for the MainToken Key."};
         }
         return new JsonResult(jsonResult);

    }

    static public string Hash(string value) 
    { 
        var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(value)); 
        return String.Concat(Array.ConvertAll(hash, x => x.ToString("X2"))); 
    }   
}
