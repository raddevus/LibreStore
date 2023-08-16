using Microsoft.AspNetCore.Mvc;
using LibreStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace LibreStore.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : Controller
{
    
    private readonly IConfiguration _config;
    private string dbType;

    public DataController(IConfiguration _configuration){
        _config = _configuration;
        dbType = _config["dbType"];
        Console.WriteLine($"###### {dbType} ##########");
    }

    // TODO: research using mock to solve this later
    // public DataController(){
    //     _config = new object() as IConfiguration;
    //     dbType = "";
    // }

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
        DataDbProvider dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        var mainTokenId = dbp.WriteUsage("SaveData",HelperTool.GetIpAddress(Request),key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        Bucket b = new Bucket(mainTokenId,intent,data,hmac,iv);
        dbp.ConfigureBucket(b);
        
        var bucketId = dbp.Save();
    
        var jsonResult = new {success=true,BucketId=bucketId};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key, Int64 bucketId){
        
        DataDbProvider dbProvider = new DataDbProvider(HelperTool.GetDbType(dbType));
        // When we call WriteUsage for GetData, we don't want to create a new MainToken
        // if it already doesn't exist, so we make last param = false
        dbProvider.WriteUsage("GetData", HelperTool.GetIpAddress(Request),key,false);

        dbProvider = new DataDbProvider(HelperTool.GetDbType(dbType));
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
        DataDbProvider dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        var mainTokenId = dbp.WriteUsage("GetBucketIds",HelperTool.GetIpAddress(Request), key,false);

        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve any data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
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
        
        DataDbProvider dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        if (HelperTool.Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            dbp.WriteUsage("GetAllTokens - rejected",HelperTool.GetIpAddress(Request),"",false);
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        dbp.WriteUsage("GetAllTokens",HelperTool.GetIpAddress(Request),"",false);
        dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        allTokens = dbp.GetAllTokens();

        return new JsonResult(allTokens);
    }

    [HttpGet("DeleteData")]
    public ActionResult DeleteData(String key, long bucketId)
    {
        DataDbProvider dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        var mainTokenId = dbp.WriteUsage("DeleteData",HelperTool.GetIpAddress(Request),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't delete Data because of invalid MainToken.Key. Data not deleted!"};
            return new JsonResult(jsonErrorResult);
        }

        dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
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
        DataDbProvider dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
        var mainTokenId = dbp.WriteUsage("UpdateOwner",HelperTool.GetIpAddress(Request),key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't add an owner because of invalid MainToken.Key. Please make sure you're using a valid MainToken Key!"};
            return new JsonResult(jsonErrorResult);
        }
        dbp = new DataDbProvider(HelperTool.GetDbType(dbType));
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
}
