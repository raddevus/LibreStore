using LibreStore.Models;
using Microsoft.Data.SqlClient;
public class SqlServerProvider
{
    public SqlConnection Connection;
    public SqlCommand Command{get;set;}
        
    /// <summary>
    /// Make sure you set the password in the connection!
    /// </summary>
    /// <param name="connectionDetails"></param>
    public SqlServerProvider( String connectionDetails = "Server=172.17.0.2;Initial Catalog=LibreStore;User ID=sa;Password=;Encrypt=False;")
    {
        Connection = new SqlConnection(connectionDetails);
        Command = Connection.CreateCommand();
    }

    public int ConfigureMainTokenInsert(String mtKey){
        String sqlCommand = @"insert into maintoken ([Key])
                select @key
                where not exists 
                (select [Key] from maintoken where [Key]=@key);
                    select id from maintoken where [Key]=@key and active=1";
        
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("@key",mtKey);
        return 0;
    }

    public int ConfigureMainTokenSelect(String mtKey){
        String sqlCommand = @"select id from maintoken
                where [Key] = @key and active=1";
        
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("@key",mtKey);
        return 0;
    }

    public int ConfigureUsage(Usage usage){
        Command.CommandText = @"INSERT into Usage (maintokenid,ipaddress,action)values(@MainTokenId,@IPAddress,@Action)";
        // Console.WriteLine($"usage.MainTokenId: {usage.MainTokenId}");
        Command.Parameters.AddWithValue("@MainTokenId",usage.MainTokenId);
        Command.Parameters.AddWithValue("@IpAddress",usage.IpAddress);
        Command.Parameters.AddWithValue("@Action", usage.Action);
        return 0;
    }
}