using LibreStore.Models;

public class DbProvider: IPersistable, IDbProvider{
    
    public IDbProvider dbProvider; 

    public DbProvider(DbType dbType, String connectionDetails = "")
    {
        switch (dbType){
            case DbType.Sqlite:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Data Source=librestore.db";
                }
                dbProvider = new SqliteProvider(connectionDetails);
                break;
            }
            case DbType.SqlServer:{
                dbProvider = new SqlServerProvider(connectionDetails);
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

   
}