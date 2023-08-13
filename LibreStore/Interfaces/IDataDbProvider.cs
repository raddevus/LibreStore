using LibreStore.Models;
public interface IDataDbProvider: IDbProvider{
    int ConfigureBucket(Bucket bucket);
    int ConfigureBucketSelect(String key, Int64 bucketId);
    int ConfigureBucketIdSelect(long mainTokenId);
    int ConfigureBucketDelete(long bucketId, long mainTokenId);
    int ConfigureMainTokenInsert(String mtKey);
    int ConfigureMainTokenSelect(String mtKey);
    int ConfigureOwnerInsert(String email);
    int ConfigureUsage(Usage u);
    int ConfigureUpdateOwner(MainToken mainToken);
    Int32 DeleteBucket();
    List<long> GetAllBucketIds();
    Bucket GetBucket();
    Int64 UpdateOwner();
    List<MainToken> GetAllTokens();
}