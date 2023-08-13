using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteDataProvider : SqliteProvider, IDataDbProvider{

    public SqliteDataProvider(String connectionDetails): base(connectionDetails)
    {
        
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

    public int ConfigureOwnerInsert(String email){
        command.CommandText = @"insert into Owner (email)  
                select $email 
                where not exists 
                (select email from owner where email=$email);
                    select id from owner where email=$email and active=1";
        command.Parameters.AddWithValue("$email",email);
        return 0; // success
    }

    public int ConfigureUpdateOwner(MainToken mainToken){
        // 2023-06-01 Discovered the sqlite Returning clause -- Returns value(s) after update or insert.
        // See https://www.sqlite.org/lang_returning.html
        String sqlCommand = @"update maintoken set OwnerId = $ownerId where key = $key and active=1 Returning ID";
        command.CommandText = sqlCommand;
        command.Parameters.AddWithValue("$ownerId", mainToken.OwnerId);
        command.Parameters.AddWithValue("$key", mainToken.Key);
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