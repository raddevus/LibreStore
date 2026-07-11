using System.Data.Common;
using LibreStore.Models;

public class CyaDbProvider: ICyaDbProvider{
    public DbCommand DbCommand { get ; set; }
    public DbConnection DbConnection { get ; set; }

    public ICyaDbProvider dbProvider; 
    public CyaDbProvider(DbType dbType, String connectionDetails = "")
    {
        switch (dbType){
            case DbType.Sqlite:{
                if (String.IsNullOrEmpty(connectionDetails)){
                   Console.WriteLine($" CYADBPROVIDER **** ONLY FIRES IF connetionDetails NOT set ****");
                    connectionDetails = "Data Source=librestore.db";
                }
                if (!Directory.Exists(Path.GetDirectoryName(connectionDetails))) {Directory.CreateDirectory(Path.GetDirectoryName(connectionDetails));}
                if (!File.Exists(connectionDetails)){
                      File.Create(connectionDetails);
                      SqliteTableBuilder s = new (connectionDetails);
                      s.CreateTables();
                }
                connectionDetails = $"Data Source={connectionDetails}";
                Console.WriteLine($"SQLITE TARGET : {connectionDetails}");
                dbProvider = new SqliteCyaProvider(connectionDetails);
                break;
            }
            case DbType.SqlServer:{
/*                if (String.IsNullOrEmpty(connectionDetails)){
                    connectionDetails = "Server=172.17.0.2;Initial Catalog=LibreStore;User ID=sa;Password=;Encrypt=False;";
                } */
                dbProvider = new SqlServerCyaProvider(AppConfig.ConnectionDetails);
                break;
            }
            case DbType.Mysql:{
/*                if (String.IsNullOrEmpty(connectionDetails)){
                    connectionDetails = "Server=172.17.0.2;Database=librestore;port=3306;uid=extra;pwd=;SslMode=preferred;";
                } */
                dbProvider = new MysqlCyaProvider(AppConfig.ConnectionDetails);
                break;
            }
        }
        DbCommand = dbProvider.DbCommand;
        DbConnection = dbProvider.DbConnection;
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

    public Cya GetCyaBucket()
    {
        return dbProvider.GetCyaBucket();
    }
}
