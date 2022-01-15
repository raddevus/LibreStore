using Microsoft.Data.Sqlite;
using LibreStore.Models;
public class SqliteCyaProvider : IPersistable{

    private SqliteConnection connection;
    public SqliteCommand command{get;set;}
    
    private String [] allTableCreation = {
               @"CREATE TABLE IF NOT EXISTS [CyaBucket]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [MainTokenId] INTEGER NOT NULL,
                    [Data] NVARCHAR(40000) check(length(Data) <= 40000),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
                    [Active] BOOLEAN default(1)
                )"};

    public SqliteCyaProvider()
    {
        try{
                connection = new SqliteConnection("Data Source=librestore.db");
                // ########### FYI THE DB is created when it is OPENED ########
                connection.Open();
                command = connection.CreateCommand();
                FileInfo fi = new FileInfo("librestore.db");
                if (fi.Length == 0){
                    foreach (String tableCreate in allTableCreation){
                        command.CommandText = tableCreate;
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine(connection.DataSource);
        }
        finally{
            if (connection != null){
                connection.Close();
            }
        }
    }

    public int GetOrInsert(){
        try{
            Console.WriteLine("GetOrInsert...");
            connection.Open();
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
            if (connection != null){
                connection.Close();
            }
        }
    }

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
                var created = "";
                if (!reader.IsDBNull(3)){
                    created = reader.GetString(3);
                }
                var updated = "";
                if (!reader.IsDBNull(4)){
                    updated = reader.GetString(4);
                }
                var active = reader.GetBoolean(5);
                Cya c = new Cya(id,mainTokenId,data,created,updated,active);
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
    
    public Int64 Save(){
        
        try{
            Console.WriteLine("Saving...");
            connection.Open();
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
            if (connection != null){
                connection.Close();
            }
        }
    }
}