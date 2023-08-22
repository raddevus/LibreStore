
using System.Data.Common;
using LibreStore.Models;
public class SqlServerDataProvider : SqlServerProvider, IDataDbProvider{

    public DbCommand DbCommand{get;set;}
    public DbConnection DbConnection { get ; set; }
    public SqlServerDataProvider(String connectionDetails): base(connectionDetails)
    {
        DbCommand = Command;
        DbConnection = Connection;
    }

    public int ConfigureBucket(Bucket bucket)
    {
        Command.CommandText = @"INSERT into Bucket (mainTokenId,intent,data,hmac,iv)values(@mainTokenId,@intent,@data,@hmac,@iv);SELECT  @@IDENTITY";
        Command.Parameters.AddWithValue("@mainTokenId",bucket.MainTokenId);
        Command.Parameters.AddWithValue("@intent",(object)bucket.Intent ?? System.DBNull.Value);
        Command.Parameters.AddWithValue("@data",bucket.Data);
        Command.Parameters.AddWithValue("@hmac",bucket.Hmac);
        Command.Parameters.AddWithValue("@iv",bucket.Iv);
        return 0;
    }

    public int ConfigureBucketDelete(long bucketId, long mainTokenId)
    {
        Command.CommandText =
            @"delete from bucket
                where mainTokenId = @tokenId
                and id = @id";
        Command.Parameters.AddWithValue("@tokenId",mainTokenId);
        Command.Parameters.AddWithValue("@id", bucketId);
        return 0;
    }

    public int ConfigureBucketIdSelect(long mainTokenId)
    {
        Command.CommandText =
                    @"select Id from bucket where MainTokenId = @id";
        Command.Parameters.AddWithValue("@id",mainTokenId);
        return 0;
    }

    public int ConfigureBucketSelect(string key, long bucketId)
    {
        Command.CommandText = @"select b.* from MainToken as mt 
                join bucket as b on mt.id = b.mainTokenId 
                where mt.[Key]=@key and b.Id = @id
                and b.active = 1 and mt.active=1";
        Command.Parameters.AddWithValue("@key",key);
        Command.Parameters.AddWithValue("@id",bucketId);
        return 0;
    }

    public int ConfigureOwnerInsert(string email)
    {
         Command.CommandText = @"insert into Owner (email)  
                select @email 
                where not exists 
                (select email from owner where email=@email);
                    select id from owner where email=@email and active=1";
        Command.Parameters.AddWithValue("@email",email);
        return 0; // success
    }

    public int ConfigureUpdateOwner(MainToken mainToken)
    {
        String sqlCommand = @"update maintoken set OwnerId = @ownerId where [key] = @key and active=1;select ID from maintoken where [key] = @key;";
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("@ownerId", mainToken.OwnerId);
        Command.Parameters.AddWithValue("@key", mainToken.Key);
        return 0;
    }

    public List<long> GetAllBucketIds()
    {
        List<long> allBucketIds = new List<long>();
        try{
            Connection.Open();
            using (var reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt64(0);
                    
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
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public List<MainToken> GetAllTokens()
    {
        Command.CommandText = "Select * from MainToken";
        List<MainToken> allTokens = new List<MainToken>();
        try{
            Connection.Open();
            using (var reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt64(0);
                    var ownerId = reader.GetInt64(1);
                    var key = reader.GetString(2);
                    var created = reader.GetString(3);
                    var active = reader.GetBoolean(4);
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
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public Bucket GetBucket()
    {
                try{
            Console.WriteLine("GetBucket...");
            Connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = Command.ExecuteReader())
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
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public long UpdateOwner()
    {
        try{
            Console.WriteLine("Updating OwnerId...");
            Connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(Command.ExecuteScalar());
            Console.WriteLine("inserted.");
            return id;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (Connection != null){
                Connection.Close();
            }
        }
    }
    
}