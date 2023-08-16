using Microsoft.Data.Sqlite;
using LibreStore.Models;

public class SqliteProvider {

    protected SqliteConnection Connection;
    public SqliteCommand Command{get;set;}
        
    public SqliteProvider( String connectionDetails = "Data Source=librestore.db")
    {
        Connection = new SqliteConnection(connectionDetails);
        Command = Connection.CreateCommand();
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
        
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("$key",mtKey);
        return 0;
    }

    public int ConfigureMainTokenSelect(String mtKey){
        String sqlCommand = @"select id from maintoken
                where key = $key and active=1";
        
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("$key",mtKey);
        return 0;
    }

    public int ConfigureUsage(Usage usage){
        Command.CommandText = @"INSERT into Usage (maintokenid,ipaddress,action)values($mainTokenId,$ipaddress,$action)";
        // Console.WriteLine($"usage.MainTokenId: {usage.MainTokenId}");
        Command.Parameters.AddWithValue("$mainTokenId",usage.MainTokenId);
        Command.Parameters.AddWithValue("$ipaddress",usage.IpAddress);
        Command.Parameters.AddWithValue("$action", usage.Action);
        return 0;
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

    public Int32 DeleteBucket(){
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