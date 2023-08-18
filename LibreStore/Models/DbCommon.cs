using System.Data.Common;
using LibreStore.Models;

public class DbCommon : IDbCommon
{
    public DbConnection Connection { get; set ; }
    public DbCommand Command { get; set; }

    public IDataDbProvider dbProvider;
    public DbCommon(DbType dbType)
    {
        var dbCommonConn = new DbCommonConnection(dbType);
        Connection = dbCommonConn.Connection;
        Command = dbCommonConn.Command;
        dbProvider = dbCommonConn.dbProvider;
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
        this.Save();
        return mainTokenId;
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            Connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = Command.ExecuteReader())
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
            if (Connection != null){
                Connection.Close();
            }
        }
    }

 public Int64 Save(){
        
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