using LibreStore.Models;
public interface ICyaDbProvider: IDbProvider{
    int Configure(Cya cya);
    int ConfigureDelete(long mainTokenId);
    int ConfigureSelect(long mainTokenId);
    Cya GetCyaBucket();
    Int32 DeleteBucket();
    Int64 Save();
	Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
}