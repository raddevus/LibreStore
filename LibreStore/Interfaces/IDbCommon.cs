using System.Data.Common;
using Microsoft.Data;

public interface IDbCommon {
    DbCommand DbCommand{get;set;}
    DbConnection DbConnection{get;set;}
}