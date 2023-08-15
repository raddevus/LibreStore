using LibreStore.Models;

public class SqlServerCyaProvider : SqlServerProvider, ICyaDbProvider
{
     public SqlServerCyaProvider(String connectionDetails) :base(connectionDetails)
    {
        
    }

    public int Configure(Cya cya)
    {
        throw new NotImplementedException();
    }

    public int ConfigureDelete(long mainTokenId)
    {
        throw new NotImplementedException();
    }

    public int ConfigureSelect(long mainTokenId)
    {
        throw new NotImplementedException();
    }

    public Cya GetCyaBucket()
    {
        throw new NotImplementedException();
    }
}