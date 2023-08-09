public class DbProvider: IPersistable{
    
    public DbProvider dbProvider; 

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
    
    long IPersistable.Save()
    {
        throw new NotImplementedException();
    }

}