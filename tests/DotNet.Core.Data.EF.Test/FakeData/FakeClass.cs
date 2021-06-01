using System.Data;
using DotNet.Core.Data.EF.Persistence.AdoDataAccess.Interfaces;
using Microsoft.Data.SqlClient;

namespace DotNet.Core.Data.EF.Test.FakeData
{
  /// <summary>
  /// 
  /// Fake class for AdoContext testing
  /// 
  /// </summary>
  public class FakeClass
  {
    private readonly IAdoContext _context;

    public FakeClass(IAdoContext context)
    {
      _context = context;
    }

    public int GetTotalCount()
    {
      var value = (int)_context.GetScalarValue("SELECT COUNT(*) FROM FAKETABLE", CommandType.Text, null);
      return value;
    }

    public IDbConnection GetDbConnection()
    {
      return _context.GetDatabaseConnection();
    }

    public SqlParameter GetSqlParameter(string name, object value, DbType dbType)
    {
      return (SqlParameter)_context.CreateParameter(name, value, dbType);
    }

    public DataTable GetTable(string name, CommandType commandType, IDbDataParameter[] parameters = null)
    {
      return _context.GetDataTable(name, commandType, null);
    }

    public DataSet GetDataSet(string name, CommandType commandType, IDbDataParameter[] parameters = null)
    {
      return _context.GetDataSet(name, commandType, null);
    }

    public long InsertRecords(string name, CommandType commandType, IDbDataParameter[] parameters)
    {
      return _context.Insert(name, commandType, parameters);
    }

    public int InsertRecordsWithTransaction(string name, CommandType commandType, IDbDataParameter[] parameters)
    {
      return _context.InsertWithTransaction(name, commandType, parameters);
    }

    public int InsertRecordsWithTransaction(string name, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
    {
      return _context.InsertWithTransaction(name, commandType, isolationLevel, parameters);
    }

    public void InsertRecords(string name, CommandType commandType, IDbDataParameter[] parameters, out long lastId)
    {
      _context.Insert(name, commandType, parameters, lastId: out lastId);
    }

    public object DeleteRecord(string name, CommandType commandType, IDbDataParameter[] parameters = null)
    {
      return _context.Delete(name, commandType, null);
    }

    public object UpdateRecord(string name, CommandType commandType, IDbDataParameter[] parameters = null)
    {
      return _context.Update(name, commandType, null);
    }

    public object UpdateRecordWithTransaction(string name, CommandType commandType, IDbDataParameter[] parameters = null)
    {
      return _context.UpdateWithTransaction(name, commandType, null);
    }

    public object UpdateRecordWithTransaction(string name, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters = null)
    {
      return _context.UpdateWithTransaction(name, commandType, isolationLevel, null);
    }
  }
}
