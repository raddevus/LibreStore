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

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            // sqliteProvider.command.CommandText = @"INSERT into Owner (mainTokenId,intent,data,hmac,iv)values($mainTokenId,$intent,$data,$hmac,$iv);SELECT last_insert_rowid()";
            // sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",owner.MainTokenId);
            // sqliteProvider.command.Parameters.AddWithValue("$intent",(object)bucket.Intent ?? System.DBNull.Value);
            // sqliteProvider.command.Parameters.AddWithValue("$data",bucket.Data);
            // sqliteProvider.command.Parameters.AddWithValue("$hmac",bucket.Hmac);
            // sqliteProvider.command.Parameters.AddWithValue("$iv",bucket.Iv);
            return 0; // success
        }
        return 1; // error
    }
}