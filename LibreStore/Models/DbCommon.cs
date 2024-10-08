using System.Data.Common;
using LibreStore.Models;

public class DbCommon : DbCommonConnection, IDbCommon{
    public DbCommand DbCommand { get; set; }
    public DbConnection DbConnection { get; set ; }
    private IDataDbProvider dbProvider{get;set;}

    
    public DbCommon(DbType dbType)
    {
        
        dbProvider = CreateDbConnection(dbType, AppConfig.ConnectionDetails);
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

    public Int64 DeleteBucket(DbConnection Connection, DbCommand Command){
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

    public Int64 GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            DbConnection.Open();
            Console.WriteLine("Opening...");
            using (var reader = DbCommand.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt64(0);
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