namespace LibreStore.Models;
public class UsageData{

    private IPersistable dataPersistor;

    private Usage usage;
    public UsageData(IPersistable dataPersistor, Usage usage)
    {
        this.dataPersistor = dataPersistor;
        this.usage = usage;
        
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteDataProvider sqliteProvider = dataPersistor as SqliteDataProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Usage (maintokenid,ipaddress,action)values($mainTokenId,$ipaddress,$action)";
            // Console.WriteLine($"usage.MainTokenId: {usage.MainTokenId}");
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",usage.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$ipaddress",usage.IpAddress);
            sqliteProvider.command.Parameters.AddWithValue("$action", usage.Action);
            return 0;
        }
        return 1;
    }



}