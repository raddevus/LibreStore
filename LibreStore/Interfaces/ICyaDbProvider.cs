using LibreStore.Models;
public interface ICyaDbProvider{
    int Configure(Cya cya);
    int ConfigureDelete(long mainTokenId);
    int ConfigureSelect(long mainTokenId);
    Cya GetCyaBucket();
    Int32 DeleteCyaBucket();
    Int64 Save();
}