using System.Data.Common;
using LibreStore.Models;

public class SqlServerCyaProvider : SqlServerProvider, ICyaDbProvider
{
      
    public DbCommand DbCommand { get ; set; }
    public DbConnection DbConnection { get ; set; }

    public SqlServerCyaProvider(String connectionDetails) :base(connectionDetails)
    {
        DbCommand = Command;
        DbConnection = Connection;
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