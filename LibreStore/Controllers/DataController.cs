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

    [HttpGet("SaveToken")]
    public ActionResult SaveToken(String key){

        MainToken mt = new MainToken(key);
        
        SqliteProvider sp = new SqliteProvider();
        WriteUsage(sp,"SaveToken",key);

        // Had to new up the SqliteProvider to insure it was initialized properly
        // for use with MainTokenData
        sp = new SqliteProvider();
        MainTokenData mtd = new MainTokenData(sp,mt);
        mtd.Configure();
        sp.Save();
        
        var jsonResult = new {success=true};
        return new JsonResult(jsonResult);
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
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"SaveData",key);
        // if mainTokenId == 0 then an error occurred.
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        sp = new SqliteProvider();
        Bucket b = new Bucket(mainTokenId,intent,data,hmac,iv);
        
        BucketData bd = new BucketData(sp,b);
        bd.Configure();
        var bucketId = sp.Save();
    
        var jsonResult = new {success=true,BucketId=bucketId};
        return new JsonResult(jsonResult);
    }

    private string getIpAddress(){
        return Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
    }

    [HttpGet("GetData")]
    public ActionResult GetData(String key, Int64 bucketId){
        
        IDbProvider dbProvider = new SqliteProvider("Data Source=librestore.db");
        dbProvider.WriteUsage("GetData", getIpAddress());
        return new JsonResult(new {success="true", id=100});
        SqliteProvider sp = new SqliteProvider();
        
        var mainTokenId = WriteUsage(sp,"GetData",key,false);
        sp = new SqliteProvider();
        Bucket b = new Bucket(bucketId,mainTokenId);
        BucketData bd = new BucketData(sp,b);
        bd.ConfigureSelect(key);
        b = sp.GetBucket();

        // if Bucket.Id is > 0 then a valid bucket was returned
        // otherwise there was not matching bucket (b.id == 0)
        var jsonResult = new {success=(b.Id > 0),bucket=b};
        return new JsonResult(jsonResult);
    }

    [HttpGet("GetBucketIds")]
    public ActionResult GetBucketIds(String key){
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"GetBucketIds",key,false);

        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't save data because of invalid MainToken.Key."};
            return new JsonResult(jsonErrorResult);    
        }
        sp = new SqliteProvider();
        
        BucketData bd = new BucketData(sp);
        bd.ConfigureBucketIdSelect(mainTokenId);
        List<long> allBucketIds = sp.GetAllBucketIds();
        if (allBucketIds.Count() == 0){
            return new JsonResult(new {success="false",message="No data available for that key."});
        }
        return new JsonResult(new {bucketIds=allBucketIds});
    }

    [HttpGet("GetAllTokens")]
    public ActionResult GetAllTokens(String pwd){
        List<MainToken> allTokens = new List<MainToken>();
        SqliteProvider sp = new SqliteProvider();
        if (Hash(pwd) != "86BC2CA50432385C30E2FAC2923AA6D19F7304E213DAB1D967A8D063BEF50EE1"){
            WriteUsage(sp,"GetAllTokens - rejected","",false);
            return new JsonResult(new {result="false",message="couldn't authenticate request"});
        }
        sp = new SqliteProvider();
        allTokens = sp.GetAllTokens();

        sp = new SqliteProvider();
        // just want to get IP Address of 
        WriteUsage(sp,"GetAllTokens","",false);
        
        return new JsonResult(allTokens);
    }

    [HttpGet("DeleteData")]
    public ActionResult DeleteData(String key, long bucketId)
    {
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"DeleteData",key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't retrieve Data because of invalid MainToken.Key. Data not deleted!"};
            return new JsonResult(jsonErrorResult);
        }

        SqliteProvider scp = new SqliteProvider();
        Bucket b = new Bucket(bucketId, mainTokenId);
        BucketData bd = new BucketData(scp,b);
        bd.ConfigureDelete(bucketId, mainTokenId);
        var deletedCount = scp.DeleteBucket();
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
        SqliteProvider sp = new SqliteProvider();
        var mainTokenId = WriteUsage(sp,"UpdateOwner",key,false);
        if (mainTokenId == 0){
            var jsonErrorResult = new {success=false,message="Couldn't add an owner because of invalid MainToken.Key. Please make sure you're using a valid MainToken Key!"};
            return new JsonResult(jsonErrorResult);
        }
        SqliteProvider scp = new SqliteProvider();
        Owner o = new Owner(email);
        OwnerData od = new OwnerData(scp,o);
        od.ConfigureInsert();
        o.ID = scp.Save();
        
        MainTokenData mtd = new MainTokenData(scp,new MainToken(key,o.ID));
        mtd.ConfigureUpdateOwner();
        Object? jsonResult = null;
         if (scp.UpdateOwner() == 0){
            jsonResult = new {success=false,message="Couldn't update the Owner for that MainToken Key. Please try again."};            
         }
         else{
            jsonResult = new {success=true,message="The Owner has been set for the MainToken Key."};
         }
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

    static public string Hash(string value) 
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
