using System.Data.Common;
using LibreStore.Models;
using Microsoft.Data.SqlClient;
public class SqlServerProvider : DbCommon, IDbProvider {

    
    public  DbConnection Connection{get;set;}
    public DbCommand Command{get;set;}
    public SqlCommand command{get;set;}
    

    /// <summary>
    /// Make sure you set the password in the connection!
    /// </summary>
    /// <param name="connectionDetails"></param>
    public SqlServerProvider( String connectionDetails = "Server=172.17.0.2;Initial Catalog=LibreStore;User ID=sa;Password=;Encrypt=False;")
    {
        Connection = new SqlConnection(connectionDetails);
        Command = Connection.CreateCommand();
        command = Command as SqlCommand;
    }

    public int ConfigureMainTokenInsert(String mtKey){
        String sqlCommand = @"insert into maintoken ([Key])
                select @key
                where not exists 
                (select [Key] from maintoken where [Key]=@key);
                    select id from maintoken where [Key]=@key and active=1";
        
        command.CommandText = sqlCommand;
        command.Parameters.AddWithValue("@key",mtKey);
        return 0;
    }

    public int ConfigureMainTokenSelect(String mtKey){
        String sqlCommand = @"select id from maintoken
                where [Key] = @key and active=1";
        
        command.CommandText = sqlCommand;
        command.Parameters.AddWithValue("@key",mtKey);
        return 0;
    }

    public int ConfigureUsage(Usage usage){
        command.CommandText = @"INSERT into Usage (maintokenid,ipaddress,action)values(@MainTokenId,@IPAddress,@Action)";
        // Console.WriteLine($"usage.MainTokenId: {usage.MainTokenId}");
        command.Parameters.AddWithValue("@MainTokenId",usage.MainTokenId);
        command.Parameters.AddWithValue("@IpAddress",usage.IpAddress);
        command.Parameters.AddWithValue("@Action", usage.Action);
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