public class SqlServerCyaProvider : CyaDbProvider
{
    //private  connection;
    // public SqliteCommand command{get;set;}
    public SqlServerCyaProvider(String connectionDetails) :base(DbType.SqlServer, connectionDetails)
    {
        
    }
    
}