using LibreStore.Models;

public class CyaDbProvider: ICyaDbProvider{
    
    public ICyaDbProvider dbProvider; 

    public CyaDbProvider(DbType dbType, String connectionDetails = "")
    {
        switch (dbType){
            case DbType.Sqlite:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Data Source=librestore.db";
                }
                dbProvider = new SqliteCyaProvider(connectionDetails);
                break;
            }
            case DbType.SqlServer:{
                dbProvider = new SqlServerCyaProvider(connectionDetails);
                break;
            }
        }
    }

    public int Configure(Cya cya)
    {
        return dbProvider.Configure(cya);
    }

    public int ConfigureDelete(long mainTokenId){
        return dbProvider.ConfigureDelete(mainTokenId);
    }

    public int ConfigureSelect(long mainTokenId)
    {
        return dbProvider.ConfigureSelect(mainTokenId);
    }

    public int DeleteBucket()
    {
        return dbProvider.DeleteBucket();
    }

    public Cya GetCyaBucket()
    {
        return dbProvider.GetCyaBucket();
    }

    public long Save()
    {
        return dbProvider.Save();
    }
}