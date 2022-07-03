namespace LibreStore.Models;
public class CyaData{

    private IPersistable dataPersistor;

    private Cya cya;
    public CyaData(IPersistable dataPersistor, Cya cya)
    {
        this.dataPersistor = dataPersistor;
        this.cya = cya;
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteCyaProvider sqliteProvider = dataPersistor as SqliteCyaProvider;
            
            sqliteProvider.command.CommandText = @"INSERT or REPLACE into CyaBucket (mainTokenId,data,hmac,iv)values($mainTokenId,$data);SELECT last_insert_rowid()";
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",cya.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$data",cya.Data);
            sqliteProvider.command.Parameters.AddWithValue("$hmac",cya.Hmac);
            sqliteProvider.command.Parameters.AddWithValue("$iv",cya.Iv);
            return 0;
        }
        return 1;
    }

    public int ConfigureSelect(long mainTokenId){
        if (dataPersistor != null)
        {
            SqliteCyaProvider sqliteProvider = dataPersistor as SqliteCyaProvider;
            sqliteProvider.command.CommandText = 
                @"select * from cyabucket
                    where mainTokenId = $id";
            sqliteProvider.command.Parameters.AddWithValue("$id",mainTokenId);
            return 0;
        }
        return 1;
    }
}