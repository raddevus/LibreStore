using LibreStore.Models;
public interface IDbProvider{
    Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
    int ConfigureBucket(Bucket bucket);
    int ConfigureBucketSelect(String key, Int64 bucketId);
    int ConfigureBucketIdSelect(long mainTokenId);
    List<long> GetAllBucketIds();
    Bucket GetBucket();
    Int64 Save();
    List<MainToken> GetAllTokens();
}