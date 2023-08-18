
using System.Data.Common;
using LibreStore.Models;
public class SqlServerDataProvider : SqlServerProvider, IDataDbProvider{

    public SqlServerDataProvider(String connectionDetails): base(connectionDetails)
    {
        
    }

    public int ConfigureBucket(Bucket bucket)
    {
        throw new NotImplementedException();
    }

    public int ConfigureBucketDelete(long bucketId, long mainTokenId)
    {
        throw new NotImplementedException();
    }

    public int ConfigureBucketIdSelect(long mainTokenId)
    {
        throw new NotImplementedException();
    }

    public int ConfigureBucketSelect(string key, long bucketId)
    {
        throw new NotImplementedException();
    }

    public int ConfigureOwnerInsert(string email)
    {
        throw new NotImplementedException();
    }

    public int ConfigureUpdateOwner(MainToken mainToken)
    {
        throw new NotImplementedException();
    }

    public List<long> GetAllBucketIds()
    {
        throw new NotImplementedException();
    }

    public List<MainToken> GetAllTokens()
    {
        Command.CommandText = "Select * from MainToken";
        List<MainToken> allTokens = new List<MainToken>();
        try{
            Connection.Open();
            using (var reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var ownerId = reader.GetInt32(1);
                    var key = reader.GetString(2);
                    var created = reader.GetString(3);
                    var active = reader.GetBoolean(4);
                    allTokens.Add(new MainToken(id,key,DateTime.Parse(created),ownerId,Convert.ToBoolean(active)));
                    Console.WriteLine($"key: {key}");
                }
            }
            return allTokens;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
            return allTokens;
        }
        finally{
            if (Connection != null){
                Connection.Close();
            }
        }
    }

    public Bucket GetBucket()
    {
        throw new NotImplementedException();
    }

    public long UpdateOwner()
    {
        throw new NotImplementedException();
    }
}