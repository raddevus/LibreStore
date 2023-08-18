using System.Data.Common;

public class DbCommonConnection // : IDbCommon
{
    public DbConnection Connection { get ; set; }
    public DbCommand Command { get; set; }

    public IDataDbProvider dbProvider;

    public DbCommonConnection(DbType dbType, String connectionDetails = "")
    {
        switch (dbType){
            case DbType.Sqlite:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Data Source=librestore.db";
                }
                dbProvider = new SqliteDataProvider(connectionDetails);
                Connection = (dbProvider as SqliteDataProvider).Connection;
                Command = (dbProvider as SqliteDataProvider).Command;
                break;
            }
            case DbType.SqlServer:{
                if (connectionDetails == String.Empty){
                    connectionDetails = "Server=172.17.0.2;Initial Catalog=LibreStore;User ID=sa;Password=;Encrypt=False;";
                }
                dbProvider = new SqlServerDataProvider(connectionDetails);
                Connection = (dbProvider as SqlServerDataProvider).Connection;
                Command = (dbProvider as SqlServerDataProvider).Command;
                break;
            }
        }
        
    }
}