using System.Data.Common;
using LibreStore.Models;

public class DataDbProvider: IDataDbProvider{
    
    public IDataDbProvider dbProvider;

    public DbCommand DbCommand{ get ; set; }
    public DbConnection DbConnection { get ; set; }

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
        // ###################################################
        // THIS IS THE LINE THAT INITS THE DbCommand !!!!!
        // ###################################################
        DbCommand = dbProvider.DbCommand;  // <=== This LINE INITS!!!
        DbConnection = dbProvider.DbConnection;
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

    public List<long> GetAllBucketIds(){
        return dbProvider.GetAllBucketIds();
    }

     public Bucket GetBucket()
    {
        return dbProvider.GetBucket();
    }

    public List<MainToken> GetAllTokens(){
        return dbProvider.GetAllTokens();
    }

    public Int64 UpdateOwner(){
        return dbProvider.UpdateOwner();
    }
}