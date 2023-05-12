namespace LibreStore.Models;
public class BucketData{

    private IPersistable dataPersistor;

    private Bucket bucket;
    public BucketData(IPersistable dataPersistor, Bucket bucket)
    {
        this.dataPersistor = dataPersistor;
        this.bucket = bucket;
    }

    public BucketData(IPersistable dataPersistor)
    {
        this.dataPersistor = dataPersistor;
    }

    public int Configure(){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            
            sqliteProvider.command.CommandText = @"INSERT into Bucket (mainTokenId,intent,data,hmac,iv)values($mainTokenId,$intent,$data,$hmac,$iv);SELECT last_insert_rowid()";
            sqliteProvider.command.Parameters.AddWithValue("$mainTokenId",bucket.MainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$intent",(object)bucket.Intent ?? System.DBNull.Value);
            sqliteProvider.command.Parameters.AddWithValue("$data",bucket.Data);
            sqliteProvider.command.Parameters.AddWithValue("$hmac",bucket.Hmac);
            sqliteProvider.command.Parameters.AddWithValue("$iv",bucket.Iv);
            return 0;
        }
        return 1;
    }

    public int ConfigureSelect(String key){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            sqliteProvider.command.CommandText = @"select b.* from MainToken as mt 
                    join bucket as b on mt.id = b.mainTokenId 
                    where mt.Key=$key and b.Id = $id
                    and b.active = 1 and mt.active=1";
            sqliteProvider.command.Parameters.AddWithValue("$key",key);
            sqliteProvider.command.Parameters.AddWithValue("$id",bucket.Id);
            return 0;
        }
        return 1;
    }

    public int ConfigureBucketIdSelect(long mainTokenId){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            sqliteProvider.command.CommandText =
                     @"select Id from bucket where MainTokenId = $id";
            sqliteProvider.command.Parameters.AddWithValue("$id",mainTokenId);
            return 0;
        }
        return 1;
    }

    public int ConfigureDelete(long bucketId, long mainTokenId){
        if (dataPersistor != null)
        {
            SqliteProvider sqliteProvider = dataPersistor as SqliteProvider;
            sqliteProvider.command.CommandText =
                @"delete from bucket
                    where mainTokenId = $tokenId
                    and id = $id";
            sqliteProvider.command.Parameters.AddWithValue("$tokenId",mainTokenId);
            sqliteProvider.command.Parameters.AddWithValue("$id", bucketId);
            return 0;
        }
        return 1;
    }
}