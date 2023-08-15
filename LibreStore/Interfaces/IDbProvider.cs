using System.Data.Common;
public interface IDbProvider
{
	public DbCommand Command {get;set;}
    public DbConnection Connection{get;set;}
	//Int64 Save();
	//Int64 WriteUsage(String action, String ipAddress, String key="", bool shouldInsert=true);
}