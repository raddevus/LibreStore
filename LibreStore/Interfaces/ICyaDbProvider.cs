using LibreStore.Models;
public interface ICyaDbProvider{
    Cya GetCyaBucket();
    Int32 DeleteCyaBucket();
    Int64 Save();
}