using LibreStore.Models;
public interface IDbProvider{
    Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
    int ConfigureBucket(Bucket bucket);
    int ConfigureBucketSelect(String key, Int64 bucketId);
    int ConfigureBucketIdSelect(long mainTokenId);
    int ConfigureBucketDelete(long bucketId, long mainTokenId);
    Int32 DeleteBucket();
    List<long> GetAllBucketIds();
    Bucket GetBucket();
    Int64 Save();
    List<MainToken> GetAllTokens();
}