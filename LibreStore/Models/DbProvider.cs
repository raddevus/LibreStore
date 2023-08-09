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

    long IPersistable.Save()
    {
        throw new NotImplementedException();
    }

}