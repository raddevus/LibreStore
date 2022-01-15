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
            
            sqliteProvider.command.CommandText = @"INSERT into CyaBucket (mainTokenId,data)values($mainTokenId,$data);SELECT last_insert_rowid()";
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",cya.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$data",cya.Data);
            return 0;
        }
        return 1;
    }

    public int ConfigureSelect(String key){
        if (dataPersistor != null)
        {
            SqliteCyaProvider sqliteProvider = dataPersistor as SqliteCyaProvider;
            sqliteProvider.command.CommandText = @"select c.* from MainToken as mt 
                    join cyabucket as c on mt.id = c.mainTokenId 
                    where mt.Key=$key and c.Id = $id
                    and c.active = 1 and mt.active=1";
            sqliteProvider.command.Parameters.AddWithValue("$key",key);
            sqliteProvider.command.Parameters.AddWithValue("$id",cya.Id);
            return 0;
        }
        return 1;
    }
}