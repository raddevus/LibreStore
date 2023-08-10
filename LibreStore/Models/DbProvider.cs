using LibreStore.Models;

public class DbProvider: IPersistable, IDbProvider{
    
    public IDbProvider dbProvider; 

    public DbProvider(DbType dbType, String connectionDetails)
    {
        switch (dbType){
            case DbType.Sqlite:{
                dbProvider = new SqliteProvider(connectionDetails);
                break;
            }
            case DbType.SqlServer:{

                break;
            }
        }
    }

    public long WriteUsage(string action, string ipAddress, string key = "", bool shouldInsert = true)
    {
        return dbProvider.WriteUsage(action,ipAddress,key,shouldInsert);
    }

    public int ConfigureBucketSelect(String key, Int64 bucketId){
        return dbProvider.ConfigureBucketSelect(key, bucketId);
    }

    
     public Bucket GetBucket()
    {
        return dbProvider.GetBucket();
    }

    long IPersistable.Save()
    {
        throw new NotImplementedException();
    }

   
}