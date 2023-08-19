using Microsoft.Data.Sqlite;
using LibreStore.Models;
using System.Data.Common;

public class SqliteCyaProvider : SqliteProvider, ICyaDbProvider{

    public DbCommand DbCommand { get ; set; }
    public DbConnection DbConnection { get ; set; }

    public SqliteCyaProvider(String connectionDetails): base(connectionDetails)
    {
        DbCommand = Command;
        DbConnection = Connection;
    }

    public int Configure(Cya cya){   
        Command.CommandText = @"INSERT or REPLACE into CyaBucket (mainTokenId,data,hmac,iv)values($mainTokenId,$data,$hmac,$iv);SELECT last_insert_rowid()";
        Command.Parameters.AddWithValue("$mainTokenId",cya.MainTokenId);
        Command.Parameters.AddWithValue("$data",cya.Data);
        Command.Parameters.AddWithValue("$hmac",cya.Hmac);
        Command.Parameters.AddWithValue("$iv",cya.Iv);
        return 0;
    }

    public int ConfigureDelete(long mainTokenId){
        Command.CommandText = 
            @"delete from cyabucket
                where mainTokenId = $id";
        Command.Parameters.AddWithValue("$id",mainTokenId);
        return 0;
        
    }
    public int ConfigureSelect(long mainTokenId){
        Command.CommandText = 
            @"select * from cyabucket
                where mainTokenId = $id";
        Command.Parameters.AddWithValue("$id",mainTokenId);
        return 0;
    }
    public Cya GetCyaBucket(){
        try{
            Console.WriteLine("GetCyaBucket...");
            Connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = Command.ExecuteReader())
            {
                reader.Read();
                var id = reader.GetInt64(0);
                var mainTokenId = reader.GetInt64(1);
                var data = reader.GetString(2);
                var hmac = reader.GetString(3);
                var iv = reader.GetString(4);
                var created = "";
                if (!reader.IsDBNull(5)){
                    created = reader.GetString(5);
                }
                var updated = "";
                if (!reader.IsDBNull(6)){
                    updated = reader.GetString(6);
                }
                var active = reader.GetBoolean(7);
                Cya c = new Cya(id,mainTokenId,data,
                            hmac, iv,
                            created,updated,active);
                Console.WriteLine($"GetBucket() id: {c.Id}");
                reader.Close();
                return c;
            }
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return new Cya(0,0);
        }
        finally{
            if (Connection != null){
                Connection.Close();
            }
        }
    }
}