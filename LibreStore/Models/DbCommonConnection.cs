public abstract class DbCommonConnection{
    /// <summary>
    /// Since both DbCommon and DataDbProvider use the same connetion setup,
    /// I've added this common connection factory for them so there is no 
    /// duplicate code
    /// </summary>
    /// <param name="dbType">enum containing sqlite,sqlserver,etc.</param>
    /// <param name="connectionDetails">The Connection string for the specific db type</param>
    /// <returns></returns>
    public IDataDbProvider CreateDbConnection(DbType dbType, String connectionDetails=""){
        IDataDbProvider dbProvider = null;
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
        return dbProvider;
    }
}