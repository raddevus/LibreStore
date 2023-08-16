using System.Data.Common;
using Microsoft.Data.Sqlite;
using LibreStore.Models;

public class SqliteProvider : DbCommon, IDbProvider {

    public  DbConnection Connection{get;set;}
    public DbCommand Command{get;set;}
    
    protected SqliteCommand command;
        
    public SqliteProvider( String connectionDetails = "Data Source=librestore.db")
    {
        Connection = new SqliteConnection(connectionDetails);
        Command = Connection.CreateCommand();
        command = Command as SqliteCommand;
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
}