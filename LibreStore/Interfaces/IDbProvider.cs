using LibreStore.Models;
using System.Data.Common;
public interface IDbProvider
{
	IDataDbProvider dbProvider{get;set;}
	DbCommand Command {get;set;}
    DbConnection Connection{get;set;}

	

}