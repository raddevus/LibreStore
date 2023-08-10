using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteProvider : IPersistable, IDbProvider{

    private SqliteConnection connection;
    public SqliteCommand command{get;set;}
        
    public SqliteProvider( String connectionDetails = "Data Source=librestore.db")
    {
        connection = new SqliteConnection(connectionDetails);
        command = connection.CreateCommand();
    }

     public Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true){
        
        MainTokenData mtd = new MainTokenData(this,new MainToken(key));
        
        if (shouldInsert){
            mtd.ConfigureInsert();
        }
        else{
            mtd.ConfigureSelect();
        }
        var mainTokenId = this.GetOrInsert();

        Usage u = new Usage(mainTokenId,ipAddress,action);
        UsageData ud = new UsageData(this,u);
        ud.Configure();
        this.Save();
        return mainTokenId;
    }

    public int ConfigureBucket(Bucket bucket){
        
        command.CommandText = @"INSERT into Bucket (mainTokenId,intent,data,hmac,iv)values($mainTokenId,$intent,$data,$hmac,$iv);SELECT last_insert_rowid()";
        command.Parameters.AddWithValue("$mainTokenId",bucket.MainTokenId);
        command.Parameters.AddWithValue("$intent",(object)bucket.Intent ?? System.DBNull.Value);
        command.Parameters.AddWithValue("$data",bucket.Data);
        command.Parameters.AddWithValue("$hmac",bucket.Hmac);
        command.Parameters.AddWithValue("$iv",bucket.Iv);
        return 0;
    }

    public int ConfigureBucketSelect(String key, Int64 bucketId){
       
        command.CommandText = @"select b.* from MainToken as mt 
                join bucket as b on mt.id = b.mainTokenId 
                where mt.Key=$key and b.Id = $id
                and b.active = 1 and mt.active=1";
        command.Parameters.AddWithValue("$key",key);
        command.Parameters.AddWithValue("$id",bucketId);
        return 0;
    }

    public int ConfigureBucketIdSelect(long mainTokenId){
        
        command.CommandText =
                    @"select Id from bucket where MainTokenId = $id";
        command.Parameters.AddWithValue("$id",mainTokenId);
        return 0;
    }

    public int ConfigureBucketDelete(long bucketId, long mainTokenId){

        command.CommandText =
            @"delete from bucket
                where mainTokenId = $tokenId
                and id = $id";
        command.Parameters.AddWithValue("$tokenId",mainTokenId);
        command.Parameters.AddWithValue("$id", bucketId);
        return 0;
    }

    public List<MainToken> GetAllTokens(){
        command.CommandText = "Select * from MainToken";
        List<MainToken> allTokens = new List<MainToken>();
        try{
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var ownerId = reader.GetInt32(1);
                    var key = reader.GetString(2);
                    var created = reader.GetString(3);
                    var active = reader.GetInt16(4);
                    allTokens.Add(new MainToken(id,key,DateTime.Parse(created),ownerId,Convert.ToBoolean(active)));
                    Console.WriteLine($"key: {key}");
                }
            }
            return allTokens;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return allTokens;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public List<long> GetAllBucketIds(){
        List<long> allBucketIds = new List<long>();
        try{
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    
                    allBucketIds.Add(id);
                    Console.WriteLine($"b.id: {id}");
                }
            }
            return allBucketIds;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return allBucketIds;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt32(0);
                Console.WriteLine($"GetOrInsert() id: {id}");
                reader.Close();
                return id;
            }
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public Bucket GetBucket(){
        try{
            Console.WriteLine("GetBucket...");
            connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt64(0);
                var mainTokenId = reader.GetInt64(1);
                String? intent = null;
                if (!reader.IsDBNull(2)){
                    intent = reader.GetString(2);
                }
                var data = reader.GetString(3);
                var hmac = reader.GetString(4);
                var iv = reader.GetString(5);
                var created = "";
                if (!reader.IsDBNull(6)){
                    created = reader.GetString(6);
                }
                var updated = "";
                if (!reader.IsDBNull(7)){
                    updated = reader.GetString(7);
                }
                var active = reader.GetBoolean(8);
                Bucket b = new Bucket(id,mainTokenId,intent,
                        data,hmac,iv,
                        created,updated,active);
                Console.WriteLine($"GetBucket() id: {b.Id}");
                reader.Close();
                return b;
            }
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return new Bucket(0,0);
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public Int32 DeleteBucket(){
        try{
            Console.WriteLine("DeleteCyaBucket...");
            connection.Open();
            Console.WriteLine("Opening...");
            // returns number of records deleted
            return command.ExecuteNonQuery();
            
        }
        catch(Exception ex){
            Console.WriteLine($"Error on delete: {ex.Message}");
            return -1;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
    
    public Int64 Save(){
        
        try{
            Console.WriteLine("Saving...");
            connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(command.ExecuteScalar());
            Console.WriteLine("inserted.");
            return id;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public Int64 UpdateOwner(){
        try{
            Console.WriteLine("Updating OwnerId...");
            connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(command.ExecuteScalar());
            Console.WriteLine("inserted.");
            return id;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
}