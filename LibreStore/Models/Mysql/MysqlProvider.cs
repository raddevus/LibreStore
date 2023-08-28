using LibreStore.Models;
using MySql.Data.MySqlClient;

public class MysqlProvider {

    public MySqlConnection Connection;
    public MySqlCommand Command{get;set;}
        
    public MysqlProvider( String connectionDetails = "Server=172.17.0.2;Database=LibreStore;uid=root;pwd=;Encrypt=False;")
    {
        Connection = new MySqlConnection(connectionDetails);
        Command = Connection.CreateCommand();
    }

    public int ConfigureMainTokenInsert(String mtKey){
        String sqlCommand = @"insert into MainToken (`Key`) 
                select @key from MainToken 
                where not exists  (select `Key` from MainToken where `Key`=@key); 
                select id from MainToken where `Key`=@key and active=1;";
        
        Command.CommandText = sqlCommand;
        Command.Parameters.AddWithValue("@key",mtKey);
        return 0;
    }

    public int ConfigureMainTokenSelect(String mtKey){
        String sqlCommand = @"select id from maintoken
                where `Key` = @key and active=1";
        
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