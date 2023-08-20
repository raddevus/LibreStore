using System.Data.Common;
using LibreStore.Models;

public class DbCommon : IDbCommon{
    public DbCommand DbCommand { get; set; }
    public DbConnection DbConnection { get; set ; }
    private IDataDbProvider dbProvider{get;set;}
    public DbCommon(DbType dbType, String connectionDetails="")
    {
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
        // ###################################################
        // THIS IS THE LINE THAT INITS THE DbCommand !!!!!
        // ###################################################
        DbCommand = dbProvider.DbCommand;  // <=== This LINE INITS!!!
        DbConnection = dbProvider.DbConnection;
    }

     public Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true){
        if (shouldInsert){
            dbProvider.ConfigureMainTokenInsert(key);
        }
        else{
            dbProvider.ConfigureMainTokenSelect(key);
        }
        var mainTokenId = this.GetOrInsert();

        Usage u = new Usage(mainTokenId,ipAddress,action);
        dbProvider.ConfigureUsage(u);
        this.Save(DbConnection,DbCommand);
    
        return mainTokenId;
    }

    public Int32 DeleteBucket(DbConnection Connection, DbCommand Command){
        try{
            Console.WriteLine("DeleteBucket...");
            Connection.Open();
            Console.WriteLine("Opening...");
            // returns number of records deleted
            return Command.ExecuteNonQuery();
            
        }
        catch(Exception ex){
            Console.WriteLine($"Error on delete: {ex.Message}");
            return -1;
        }
        finally{
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            DbConnection.Open();
            Console.WriteLine("Opening...");
            using (var reader = DbCommand.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt32(0);
                Console.WriteLine($"GetOrInsert() id: {id}");
                reader.Close();
                return id;
            }
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (DbConnection != null){
                DbConnection.Close();
            }
        }
    }

 public Int64 Save(DbConnection Connection, DbCommand Command){
        try{
            Console.WriteLine("Saving...");
            Connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(Command.ExecuteScalar());
            Console.WriteLine("inserted.");
            return id;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
        finally{
            if (Connection != null){
                Connection.Close();
            }
        }
    }   
}