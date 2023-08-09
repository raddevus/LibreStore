public interface IDbProvider{
    Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
}