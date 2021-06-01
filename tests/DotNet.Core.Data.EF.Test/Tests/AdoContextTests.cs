using System.Data;
using Microsoft.Data.SqlClient;
using DotNet.Core.Data.EF.Persistence.AdoDataAccess.Interfaces;
using DotNet.Core.Data.EF.Test.FakeData;
using Moq;
using Shouldly;
using Xbehave;

namespace DotNet.Core.Data.EF.Test.Tests
{
  public class AdoContextTests
  {
    private readonly Mock<IAdoContext> _contextMock;
    private readonly FakeClass _fakeClass;
    private const string FakeConnectionString = @"Data Source=TestServer;Initial Catalog=TestCatalog;User ID=admin;Password=******;";

    public AdoContextTests()
    {
      _contextMock = new Mock<IAdoContext>();
      _fakeClass = new FakeClass(_contextMock.Object);
    }

    [Scenario]
    public void GetDbConnectionTest()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x => x.GetDatabaseConnection()).Returns(new SqlConnection(FakeConnectionString));
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.GetDbConnection();
          result.ShouldBeOfType<SqlConnection>();
        });
    }

    [Scenario]
    public void CreateSqlParameter()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x => x.CreateParameter(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbType>())).Returns(GetMockSqlParameter());
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.GetSqlParameter("@abc", 1000, DbType.Int32);
          result.ShouldBeOfType<SqlParameter>();
          result.Value.ShouldBe(1000);
        });
    }

    [Scenario]
    public void GetScalarValue()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x => x.GetScalarValue(It.IsAny<string>(), CommandType.Text, null)).Returns(1);
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.GetTotalCount();
          result.ShouldBe(1);
        });
    }

    [Scenario]
    public void GetDataTable()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x => x.GetDataTable(It.IsAny<string>(), CommandType.Text, null)).Returns(GetMockDataTable());
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.GetTable(It.IsAny<string>(), CommandType.Text);
          result.Rows.Count.ShouldBe(1);
        });
    }

    [Scenario]
    public void GetDataSet()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x => x.GetDataSet(It.IsAny<string>(), CommandType.Text, null)).Returns(GetMockDataSet());
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.GetDataSet(It.IsAny<string>(), CommandType.Text);
          result.Tables.Count.ShouldBe(1);
          result.Tables["tempTable"]?.Rows.Count.ShouldBe(1);
        });
    }

    [Scenario]
    public void Inserts()
    {
      long value = 1;
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x =>
            x.Insert(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>())).Returns(1);
          _contextMock.Setup(x =>
            x.InsertWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>())).Returns(1);
          _contextMock.Setup(x =>
            x.InsertWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IsolationLevel>(), It.IsAny<IDbDataParameter[]>())).Returns(1);
          _contextMock.Setup(x =>
            x.Insert(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>(), out value));
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.InsertRecords(It.IsAny<string>(), CommandType.Text, new[] { GetMockSqlParameter() });
          result.ShouldBe(1);

          var transactionResult = _fakeClass.InsertRecordsWithTransaction(It.IsAny<string>(), CommandType.Text, new[] { GetMockSqlParameter() });
          transactionResult.ShouldBe(1);

          var isolatedTransaction = _fakeClass.InsertRecordsWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IsolationLevel>(), new[] { GetMockSqlParameter() });
          isolatedTransaction.ShouldBe(1);

          _fakeClass.InsertRecords(It.IsAny<string>(), CommandType.Text, new[] { GetMockSqlParameter() }, out value);
          value.ShouldBe(1);
        });

    }

    [Scenario]
    public void Delete()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x =>
            x.Delete(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>())).Returns(1);
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.DeleteRecord(It.IsAny<string>(), CommandType.Text,
            new[] { GetMockSqlParameter() });
          result.ShouldBe(1);
        });

    }

    [Scenario]
    public void Updates()
    {
      "Arrange"
        .x(() =>
        {
          _contextMock.Setup(x =>
            x.Update(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>())).Returns(1);
          _contextMock.Setup(x =>
            x.UpdateWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IDbDataParameter[]>())).Returns(1);
          _contextMock.Setup(x =>
            x.UpdateWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IsolationLevel>(), It.IsAny<IDbDataParameter[]>())).Returns(1);
        });

      "Act & Assert"
        .x(() =>
        {
          var result = _fakeClass.UpdateRecord(It.IsAny<string>(), CommandType.Text, new[] { GetMockSqlParameter() });
          result.ShouldBe(1);

          var transactionRecord = _fakeClass.UpdateRecordWithTransaction(It.IsAny<string>(), CommandType.Text, new[] { GetMockSqlParameter() });
          transactionRecord.ShouldBe(1);

          var isolatedTransaction = _fakeClass.UpdateRecordWithTransaction(It.IsAny<string>(), CommandType.Text, It.IsAny<IsolationLevel>(), new[] { GetMockSqlParameter() });
          isolatedTransaction.ShouldBe(1);
        });

    }
    #region Private Methods

    private IDbDataParameter GetMockSqlParameter()
    {
      return new SqlParameter
      {
        DbType = It.IsAny<DbType>(),
        ParameterName = It.IsAny<string>(),
        Value = 1000
      };
    }
    private DataSet GetMockDataSet()
    {
      var dataSet = new DataSet("Test");
      dataSet.Tables.Add(GetMockDataTable());

      return dataSet;
    }

    private DataTable GetMockDataTable()
    {
      var dt = new DataTable("tempTable");
      dt.Clear();
      dt.Columns.Add("Name");
      dt.Columns.Add("Marks");
      object[] o = { "test", 500 };
      dt.Rows.Add(o);

      return dt;
    }

    #endregion
  }
}
