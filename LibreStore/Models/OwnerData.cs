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
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Owner (email) values ($email);SELECT last_insert_rowid()";
            sqliteProvider.command.Parameters.AddWithValue("$email",owner.Email);
            return 0; // success
        }
        return 1; // error
    }
}