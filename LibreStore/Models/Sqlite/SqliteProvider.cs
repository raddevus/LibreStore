using System.Data.Common;
using Microsoft.Data.Sqlite;
using LibreStore.Models;

public class SqliteProvider : IDbProvider {

    public  DbConnection Connection{get;set;}
    public DbCommand Command{get;set;}
    
    protected SqliteCommand command;
        
    public SqliteProvider( String connectionDetails = "Data Source=librestore.db")
    {
        Connection = new SqliteConnection(connectionDetails);
        Command = Connection.CreateCommand();
        command = Command as SqliteCommand;
    }

    public Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true){
        if (shouldInsert){
            ConfigureMainTokenInsert(key);
        }
        else{
            ConfigureMainTokenSelect(key);
        }
        var mainTokenId = this.GetOrInsert();

        Usage u = new Usage(mainTokenId,ipAddress,action);
        ConfigureUsage(u);
        this.Save();
        return mainTokenId;
    }

    public int ConfigureMainTokenInsert(String mtKey){
        String sqlCommand = @"insert into maintoken (key)  
                select $key 
                where not exists 
                (select key from maintoken where key=$key);
                    select id from maintoken where key=$key and active=1";
        
        command.CommandText = sqlCommand;
        command.Parameters.AddWithValue("$key",mtKey);
        return 0;
    }

    public int ConfigureMainTokenSelect(String mtKey){
        String sqlCommand = @"select id from maintoken
                where key = $key and active=1";
        
        command.CommandText = sqlCommand;
        command.Parameters.AddWithValue("$key",mtKey);
        return 0;
    }

    public int ConfigureUsage(Usage usage){
        command.CommandText = @"INSERT into Usage (maintokenid,ipaddress,action)values($mainTokenId,$ipaddress,$action)";
        // Console.WriteLine($"usage.MainTokenId: {usage.MainTokenId}");
        command.Parameters.AddWithValue("$mainTokenId",usage.MainTokenId);
        command.Parameters.AddWithValue("$ipaddress",usage.IpAddress);
        command.Parameters.AddWithValue("$action", usage.Action);
        return 0;
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            Connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
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

    public Int32 DeleteBucket(){
        try{
            Console.WriteLine("DeleteBucket...");
            Connection.Open();
            Console.WriteLine("Opening...");
            // returns number of records deleted
            return command.ExecuteNonQuery();
            
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

    public Int64 Save(){
        
        try{
            Console.WriteLine("Saving...");
            Connection.Open();
            Console.WriteLine("Opened.");
            // id should be last id inserted into table
            var id = Convert.ToInt64(command.ExecuteScalar());
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