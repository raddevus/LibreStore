namespace LibreStore.Models;
public class MainTokenData{

    private IPersistable dataPersistor;

    private MainToken mainToken;
    public MainTokenData(IPersistable dataPersistor, MainToken mainToken)
    {
        this.dataPersistor = dataPersistor;
        this.mainToken = mainToken;
        
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into MainToken (key)values($key)";
            sqliteProvider.command.Parameters.AddWithValue("$key",mainToken.Key);
            return 0;
        }
        return 1;
    }

    public int ConfigureInsert(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            String sqlCommand = @"insert into maintoken (key)  
                    select $key 
                    where not exists 
                    (select key from maintoken where key=$key);
                     select id from maintoken where key=$key and active=1";
            
            sqliteProvider.command.CommandText = sqlCommand;
            sqliteProvider.command.Parameters.AddWithValue("$key",mainToken.Key);
            return 0;
            
        }
        return 2;
    }

    public int ConfigureSelect(){
        SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
        String sqlCommand = @"select id from maintoken
                where key = $key and active=1";
        
        sqliteProvider.command.CommandText = sqlCommand;
        sqliteProvider.command.Parameters.AddWithValue("$key",mainToken.Key);
        return 0;
    }

    public int ConfigureUpdateOwner(){
        SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
        // 2023-06-01 Discovered the sqlite Returning clause -- Returns value(s) after update or insert.
        // See https://www.sqlite.org/lang_returning.html
        String sqlCommand = @"update maintoken set OwnerId = $ownerId where key = $key and active=1 Returning ID";        
        sqliteProvider.command.CommandText = sqlCommand;
        sqliteProvider.command.Parameters.AddWithValue("$ownerId", mainToken.OwnerId);
        sqliteProvider.command.Parameters.AddWithValue("$key", mainToken.Key);
        return 0;
    }



}