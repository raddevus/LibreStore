using System.Data.Common;
using LibreStore.Models;

public class DataDbProvider: IDataDbProvider{
    
    public IDataDbProvider dbProvider;

    public DbCommand Command { get => dbProvider.Command; set => dbProvider.Command = value; }
    public DbConnection Connection { get => dbProvider.Connection; set => dbProvider.Connection = value; }

    public DataDbProvider(DbType dbType, String connectionDetails = "")
    {
        switch (dbType){
            case DbType.Sqlite:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Data Source=librestore.db";
                }
                dbProvider = new SqliteDataProvider(connectionDetails);
                break;
            }
            case DbType.SqlServer:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Server=172.17.0.2;Initial Catalog=LibreStore;User ID=sa;Password=;Encrypt=False;";
                }
                dbProvider = new SqlServerDataProvider(connectionDetails);
                break;
            }
        }
    }

    public long WriteUsage(string action, string ipAddress, string key = "", bool shouldInsert = true)
    {
        return dbProvider.WriteUsage(action,ipAddress,key,shouldInsert);
    }

    public int ConfigureBucket(Bucket bucket){
        return dbProvider.ConfigureBucket(bucket);
    }

    public int ConfigureBucketSelect(String key, Int64 bucketId){
        return dbProvider.ConfigureBucketSelect(key, bucketId);
    }

    public int ConfigureBucketIdSelect(long mainTokenId){
        return dbProvider.ConfigureBucketIdSelect(mainTokenId);
    }

    public int ConfigureBucketDelete(long bucketId, long mainTokenId){
        return dbProvider.ConfigureBucketDelete(bucketId, mainTokenId);
    }

    public int ConfigureMainTokenInsert(String mtKey){
        return dbProvider.ConfigureMainTokenInsert(mtKey);
    }
    public int ConfigureMainTokenSelect(String mtKey){
        return dbProvider.ConfigureMainTokenSelect(mtKey);
    }

    public int ConfigureOwnerInsert(String email){
        return dbProvider.ConfigureOwnerInsert(email);
    }

    public int ConfigureUsage(Usage u){
        return dbProvider.ConfigureUsage(u);
    }

    public int ConfigureUpdateOwner(MainToken mainToken){
        return dbProvider.ConfigureUpdateOwner(mainToken);
    }

    public Int32 DeleteBucket(){
        return dbProvider.DeleteBucket();
    }

    public List<long> GetAllBucketIds(){
        return dbProvider.GetAllBucketIds();
    }

     public Bucket GetBucket()
    {
        return dbProvider.GetBucket();
    }

    public long Save()
    {
        return dbProvider.Save();
    }

    public List<MainToken> GetAllTokens(){
        Command.CommandText = "Select * from MainToken";
        List<MainToken> allTokens = new List<MainToken>();
        try{
            Connection.Open();
            using (var reader = Command.ExecuteReader())
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
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public Int64 UpdateOwner(){
        return dbProvider.UpdateOwner();
    }

   
}