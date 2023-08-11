using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteCyaProvider : SqliteProvider, ICyaDbProvider{

    public Cya GetCyaBucket(){
        try{
            Console.WriteLine("GetCyaBucket...");
            connection.Open();
            Console.WriteLine("Opening...");
            using (var reader = command.ExecuteReader())
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
            if (connection != null){
                connection.Close();
            }
        }
    }

    public Int32 DeleteCyaBucket(){
        try{
            Console.WriteLine("DeleteCyaBucket...");
            connection.Open();
            Console.WriteLine("Opening...");
            // returns number of records deleted
            return command.ExecuteNonQuery();
            
        }
        catch(Exception ex){
            Console.WriteLine($"Error on delete: {ex.Message}");
            return -1;
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }
}