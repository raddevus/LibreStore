public class SqlServerProvider : DataDbProvider
{
    //private  connection;
    // public SqliteCommand command{get;set;}
    public SqlServerProvider(String connectionDetails) :base(DbType.SqlServer, connectionDetails)
    {
        
    }
    
}