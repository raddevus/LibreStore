public class SqlServerProvider : DbProvider
{
    //private  connection;
    // public SqliteCommand command{get;set;}
    public SqlServerProvider(String connectionDetails) :base(DbType.SqlServer, connectionDetails)
    {
        
    }
    long Save()
    {
        throw new NotImplementedException();

    }
}