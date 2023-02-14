using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TwoMites_Engine._01.DAO
{
  public enum EXECUTE_TYPE { GET_DATE_TABLE, EXEUCUTE }
  public class DAO_MySQL_v2 : IDisposable
  {
    public DAO_MySQL_v2()
      : this(DB_HOST, DB_PORT, DB_USER, DB_PASS, DB_NAME)
    { }
    public DAO_MySQL_v2(string _name)
  : this(DB_HOST, DB_PORT, DB_USER, DB_PASS, _name)
    { }
    public DAO_MySQL_v2(string _user, string _pass)
      : this(DB_HOST, DB_PORT, _user, _pass, DB_NAME)
    { }
    public DAO_MySQL_v2(string _host, string _port, string _user, string _pass, string _name)
    {
      host = _host;
      port = _port;
      user = _user;
      pass = _pass;
      name = _name;
      url = $"DATA SOURCE={host}; PORT={port}; DATABASE={name}; UID={user}; PASSWORD={pass}; CharSet={CHAR_SET}; Allow User Variables=TRUE;";
    }

    ~DAO_MySQL_v2()
    {
      Dispose();
    }
    private const string DB_HOST = "localhost";
    private const string DB_PORT = "3306";
    private const string DB_USER = "root";
    private const string DB_PASS = "root";
    private const string DB_NAME = "TwoMites";
    private const string CHAR_SET = "UTF8";

    private string host;
    private string port;
    private string user;
    private string pass;
    private string name;

    private string url;
    private string query;

    public object result;
    private DataTable _data_table = null;
    public DataTable m_data_table
    {
      get => _data_table;
      set
      {
        if (_data_table != null)
          _data_table.Dispose();
        _data_table = value;
      }
    }

    #region 멤버함수 선언부
    protected MySql.Data.MySqlClient.MySqlConnection ConnectDB() => connect_DB();
    protected MySql.Data.MySqlClient.MySqlCommand SetCommand(string _command, MySql.Data.MySqlClient.MySqlConnection _connect) => set_command(query, _connect);
    public void SetQuery(string _query) => set_query(_query);
    public void Execute(EXECUTE_TYPE _execute_type) => execute(_execute_type);
    public object GetListObj() => get_obj_list();
    public DataTable GetDataTable() => get_data_table();
    public DataTable GetDataTable(string _query) => get_data_table(_query);
    public bool ExecuteMulQuery(List<string> _list_query) => execute_mul_query(_list_query);

    #endregion

    #region 멤버함수 정의부
    private void set_query(string _query) => query = _query;
    private void execute(EXECUTE_TYPE _execute_type = EXECUTE_TYPE.GET_DATE_TABLE)
    {
      switch (_execute_type)
      {
        case EXECUTE_TYPE.GET_DATE_TABLE:
          get_data_table();
          break;
      }
    }
    private MySql.Data.MySqlClient.MySqlConnection connect_DB() => new MySql.Data.MySqlClient.MySqlConnection(url);
    private MySql.Data.MySqlClient.MySqlCommand set_command(string _command, MySql.Data.MySqlClient.MySqlConnection _connect) => new MySql.Data.MySqlClient.MySqlCommand(query, _connect);


    #endregion

    #region 쿼리 별 실행 유형 
    private object get_obj_list()
    {
      List<List<object>> list_data_table = null;
      try
      {
        using (var connection = connect_DB())
        using (var command = set_command(query, connection))
        {
          connection.Open();
          MySql.Data.MySqlClient.MySqlDataAdapter data_adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(command);
          using (m_data_table = new DataTable())
          {
            if (data_adapter.Fill(m_data_table) > 0)
            {
              list_data_table = new List<List<object>>(m_data_table.Rows.Count);
              var size = m_data_table.Columns.Count;
              foreach (DataRow row in m_data_table.Rows)
              {
                list_data_table.Add(new List<object>());
                for (int i = 0; i < size; i++)
                  list_data_table[list_data_table.Count - 1].Add(row[i]);
              }
            }
          }
        }
      }
      catch (Exception _e)
      {
        System.Diagnostics.Debug.WriteLine($"예외 == {_e.Message}");
      }
      return result = list_data_table;
    }


    private DataTable get_data_table(string _query)
    {
      set_query(_query);
      return get_data_table();
    }
    private DataTable get_data_table()
    {

      try
      {
        using (var connection = connect_DB())
        using (var command = set_command(query, connection))
        {
          connection.Open();
          using (MySql.Data.MySqlClient.MySqlDataAdapter data_adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(command))
          using (m_data_table = new DataTable())
            if (data_adapter.Fill(m_data_table) > 0)
              return m_data_table;
        }
      }
      catch (Exception _e)
      {
        System.Diagnostics.Debug.WriteLine($"예외 == {_e.Message}");
      }
      return null;
    }

    public void Dispose()
    {
      using (result as DataTable)
      {
        ((DataTable)result)?.Dispose();
        //GC.SuppressFinalize(true);
      }
      if (m_data_table?.Rows.Count != 0)
      {
        m_data_table?.Clear();
        m_data_table?.Dispose();
      }
    }

    bool execute_mul_query(List<string> _list_query)
    {
      _list_query.ForEach(_query => get_data_table(_query));
      return true;
    }
    #endregion

    #region DB 백업 및 Restore
    public void BackUpDB(string path)
    {
      try
      {
        using (var connection = connect_DB())
        using (var command = new MySql.Data.MySqlClient.MySqlCommand())
        using (var mysql_backup = new MySql.Data.MySqlClient.MySqlBackup(command))
        {
          connection.Open();
          command.Connection = connection;
          mysql_backup.ExportToFile(path);
          connection.Close();
        }
      }
      catch (Exception _e)
      {
        System.Diagnostics.Debug.WriteLine(_e.Message);
      }
    }

    public void RestoreDB(string path)
    {
      try
      {
        using (var connection = connect_DB())
        using (var command = new MySql.Data.MySqlClient.MySqlCommand())
        using (var mysql_backup = new MySql.Data.MySqlClient.MySqlBackup(command))
        {
          connection.Open();
          command.Connection = connection;
          mysql_backup.ImportFromFile(path);
          connection.Close();
        }
      }
      catch (Exception _e)
      {
        System.Diagnostics.Debug.WriteLine(_e.Message);
      }

    }
    #endregion
  }
}