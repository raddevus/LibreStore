using System.Data.Common;
using Microsoft.Data;

public interface IDbCommon {
    // DbConnection Connection{get;set;}
    DbCommand DbCommand{get;set;}
}