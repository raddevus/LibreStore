
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
        throw new NotImplementedException();
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