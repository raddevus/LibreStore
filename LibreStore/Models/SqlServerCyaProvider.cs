public class SqlServerCyaProvider : DbProvider
{
    //private  connection;
    // public SqliteCommand command{get;set;}
    public SqlServerCyaProvider(String connectionDetails) :base(DbType.SqlServer, connectionDetails)
    {
        
    }
    
}