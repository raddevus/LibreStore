using System.Data;
using System.Data.Common;
using LibreStore.Models;
public class DbProvider : IDbProvider {

    public IDataDbProvider dbProvider; 
    public DbCommand Command{get;set;}
    public DbConnection Connection{get;set;}

    public DbProvider(DbConnection connection, DbCommand command)
    {
        Connection = connection;
        Command = command;
    }
    
    // public Int64 Save(){
    //     try{
    //         Console.WriteLine("Saving...");
    //         Connection.Open();
    //         Console.WriteLine("Opened.");
    //         // id should be last id inserted into table
    //         var id = Convert.ToInt64(Command.ExecuteScalar());
    //         Console.WriteLine("inserted.");
    //         return id;
    //     }
    //     catch(Exception ex){
    //         Console.WriteLine($"Error: {ex.Message}");
    //         return 0;
    //     }
    //     finally{
    //         if (Connection != null){
    //             Connection.Close();
    //         }
    //     }
    // }
    // public List<long> GetAllBucketIds(){
    //     List<long> allBucketIds = new List<long>();
    //     try{
    //         Connection.Open();
    //         using (var reader = Command.ExecuteReader())
    //         {
    //             while (reader.Read())
    //             {
    //                 var id = reader.GetInt32(0);
                    
    //                 allBucketIds.Add(id);
    //                 Console.WriteLine($"b.id: {id}");
    //             }
    //         }
    //         return allBucketIds;
    //     }
    //     catch(Exception ex){
    //         Console.WriteLine($"Error: {ex.Message}");
    //         return allBucketIds;
    //     }
    //     finally{
    //         if (Connection != null){
    //             Connection.Close();
    //         }
    //     }
    // }
    // public List<MainToken> GetAllTokens(){
    //     Command.CommandText = "Select * from MainToken";
    //     List<MainToken> allTokens = new List<MainToken>();
    //     try{
    //         Connection.Open();
    //         using (var reader = Command.ExecuteReader())
    //         {
    //             while (reader.Read())
    //             {
    //                 var id = reader.GetInt32(0);
    //                 var ownerId = reader.GetInt32(1);
    //                 var key = reader.GetString(2);
    //                 var created = reader.GetString(3);
    //                 var active = reader.GetInt16(4);
    //                 allTokens.Add(new MainToken(id,key,DateTime.Parse(created),ownerId,Convert.ToBoolean(active)));
    //                 Console.WriteLine($"key: {key}");
    //             }
    //         }
    //         return allTokens;
    //     }
    //     catch(Exception ex){
    //         Console.WriteLine($"Error: {ex.Message}");
    //         return allTokens;
    //     }
    //     finally{
    //         if (Connection != null){
    //             Connection.Close();
    //         }
    //     }
    //  }

    //  public Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true){
    //     if (shouldInsert){
    //         dbProvider.ConfigureMainTokenInsert(key);
    //     }
    //     else{
    //         dbProvider.ConfigureMainTokenSelect(key);
    //     }
    //     var mainTokenId = this.GetOrInsert();

    //     Usage u = new Usage(mainTokenId,ipAddress,action);
    //     dbProvider.ConfigureUsage(u);
    //     this.Save();
    //     return mainTokenId;
    // }

    // public int GetOrInsert(){
    //     try{
    //         Console.WriteLine("GetOrInsert...");
    //         Connection.Open();
    //         Console.WriteLine("Opening...");
    //         using (var reader = Command.ExecuteReader())
    //         {
    //             reader.Read();
    //             var id = reader.GetInt32(0);
    //             Console.WriteLine($"GetOrInsert() id: {id}");
    //             reader.Close();
    //             return id;
    //         }
    //     }
    //     catch(Exception ex){
    //         Console.WriteLine($"Error: {ex.Message}");
    //         return 0;
    //     }
    //     finally{
    //         if (Connection != null){
    //             Connection.Close();
    //         }
    //     }
    // }    
}