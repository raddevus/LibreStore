namespace LibreStore.Models;
using Microsoft.Data.Sqlite;
public class SqliteTableBuilder{

    private readonly SqliteConnection connection;
    public SqliteCommand Command{get;set;}


    private readonly String [] allTableCreation = {
                @"CREATE TABLE IF NOT EXISTS [MainToken]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY,
                    [OwnerId] INTEGER NOT NULL default(0),
                    [Key] NVARCHAR(128)  NOT NULL UNIQUE check(length(Key) <= 128) check(length(key) >= 10),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Active] BOOLEAN default (1)
                )",

                @"CREATE TABLE IF NOT EXISTS [Bucket]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY,
                    [MainTokenId] INTEGER NOT NULL,
                    [Intent] NVARCHAR(20) check(length(Intent) <= 20),
                    [Data] NVARCHAR(20000) NOT NULL check(length(Data) <= 20000),
                    [Hmac] NVARCHAR(64) NOT NULL check(length(Hmac) <= 64),
                    [Iv] NVARCHAR(32) NOT NULL check(length(Iv) <= 32),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
                    [Active] BOOLEAN default(1)
                )",

                @"CREATE TABLE IF NOT EXISTS [Usage]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY,
                    [MainTokenId] INTEGER NOT NULL default(0),
                    [IpAddress] NVARCHAR(60) check(length(IpAddress) <= 60),
                    [Action] NVARCHAR(75) check(length(Action) <= 75),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Active] BOOLEAN default (1)
                )",
                @"CREATE TABLE IF NOT EXISTS [Owner]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY,
                    [Email] NVARCHAR(200) UNIQUE check(length(Email) <= 200),
                    [ExpireDate] NVARCHAR(30) check(length(Created) <= 30),
                    [Subscribed] BOOLEAN default(0),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
                    [Active] BOOLEAN default(1)
                )",
                @"CREATE TABLE IF NOT EXISTS [CyaBucket]
                (
                    [ID] INTEGER NOT NULL PRIMARY KEY,
                    [MainTokenId] INTEGER NOT NULL UNIQUE,
                    [Data] NVARCHAR(40000) NOT NULL check(length(Data) <= 40000),
                    [Hmac] NVARCHAR(64) NOT NULL check(length(Hmac) <= 64),
                    [Iv] NVARCHAR(32) NOT NULL check(length(Iv) <= 32),
                    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
                    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
                    [Active] BOOLEAN default(1)
                )" 
            };
    
    public SqliteTableBuilder()
    {

        try{
            connection = new SqliteConnection("Data Source=librestore.db");
            // ########### FYI THE DB is created when it is OPENED ########
            connection.Open();
            Command = connection.CreateCommand();
            FileInfo fi = new FileInfo("librestore.db");
            if (fi.Length == 0){
                //
                Console.WriteLine("Adding all tables to librestore.db");
                foreach (String tableCreate in allTableCreation){
                    Command.CommandText = tableCreate;
                    Command.ExecuteNonQuery();
                }
            }
            Console.WriteLine(connection.DataSource);
        }
        finally{
            connection?.Close();
        }
    }
        
}