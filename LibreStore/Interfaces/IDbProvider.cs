using LibreStore.Models;
public interface IDbProvider{
    Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
    int ConfigureBucket(Bucket bucket);
    int ConfigureBucketSelect(String key, Int64 bucketId);
    public int ConfigureBucketIdSelect(long mainTokenId);
    public List<long> GetAllBucketIds();
    public Bucket GetBucket();
    public Int64 Save();
}