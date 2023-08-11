namespace LibreStore.Models;

public class OwnerData{

    private IPersistable dataPersistor;

    private Owner owner;
    public OwnerData(IPersistable dataPersistor, Owner owner)
    {
        this.dataPersistor = dataPersistor;
        this.owner = owner;
    }

    public OwnerData(IPersistable dataPersistor)
    {
        this.dataPersistor = dataPersistor;
    }

    public int ConfigureInsert(){
        if (dataPersistor != null)
        {
            SqliteDataProvider sqliteProvider = dataPersistor as SqliteDataProvider;
                        
            sqliteProvider.command.CommandText = @"insert into Owner (email)  
                    select $email 
                    where not exists 
                    (select email from owner where email=$email);
                     select id from owner where email=$email and active=1";
            sqliteProvider.command.Parameters.AddWithValue("$email",owner.Email);
            return 0; // success
        }
        return 1; // error
    }
}