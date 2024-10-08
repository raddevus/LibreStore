using LibreStore.Models;
public interface ICyaDbProvider: IDbCommon{
    int Configure(Cya cya);
    int ConfigureDelete(long mainTokenId);
    int ConfigureSelect(long mainTokenId);
    Cya GetCyaBucket();
}