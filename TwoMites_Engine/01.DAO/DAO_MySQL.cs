using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._01.DAO
{
  public enum QUERY { NORMAL, REPLACE, END }
  public delegate string del_func(List<string> _list_str);
  public class DAO_MySQL : IDisposable
  {
    public DAO_MySQL()
    {
      _Init();
      initialize();
    }
    public DAO_MySQL(string _db_name, string _db_pass)
        : this()
    {
      DB_NAME = _db_name;
      DB_PASS = _db_pass;
    }
    ~DAO_MySQL()
    {
      Dispose();
    }
    const string DB_HOST = "localhost;";
    const string DB_USER = "root;";
    readonly string DB_NAME = "TwoMites;";
    readonly string DB_PASS = "root;";
    const string CHAR_SET = "UTF8;";

    private string? m_db_name;
    private string? m_db_pass;

    private MySqlConnection? m_mySqlConnection;
    private MySqlCommand? m_mySqlCommand;
    private MySqlDataReader? m_mySqlDataReader;


    private string? m_connectionPath;
    private int m_column_cnt;
    private string? m_str_query;
    private Dictionary<QUERY, del_func>? m_map_query_setter;
    private List<string> m_list_value_of_question_mark;
    private List<string>[] m_list_result_receiver;

    private void initialize()
    {
      initialize_query_setter();
    }
    private void _Init()
    {
      m_mySqlConnection = null;

      m_db_name = null;
      m_db_pass = null;
      this?.Dispose();
      m_mySqlConnection = null;
      m_mySqlCommand = null;
      m_mySqlDataReader = null;


      m_connectionPath = null;
      m_column_cnt = 0;
      m_str_query = null;

      m_list_value_of_question_mark = new List<string>();
    }
    private void initialize_query_setter()
    {
      m_map_query_setter = new Dictionary<QUERY, del_func>();
      m_map_query_setter.Add(QUERY.NORMAL, (List<string> _list_query) => { return _list_query[0]; });
      m_map_query_setter.Add(QUERY.REPLACE,
        (List<string> _list_query) =>
        {
          const int DEFAULT_CAP = 512;
          StringBuilder str_buil = new StringBuilder(DEFAULT_CAP);
          //글자 하나씩 insert 하다가 ? 나오면 ? 대신 _list_query 넣기
          int current_element = 1;
          for (int i = 0; i < _list_query[0].Length; ++i)
          {
            char ch = _list_query[0][i];
            if (ch != '?')
              str_buil.Append(ch);
            else
            {
              str_buil.Append(_list_query[current_element]);
              ++current_element;
            }
          }
          return str_buil.ToString();
        });
    }

    public void set_column_cnt(int _i) { m_column_cnt = _i; }

    public void execute(QUERY _eQuery)
    {
      //쿼리문에 담긴 (')작은 따옴표를 ('')작은 따옴표 두 개로 바꾸는 함수
      remove_single_quote();

      //DB 통로 열기
      connect_DB();

      //쿼리문 만들기
      m_str_query = m_map_query_setter[_eQuery]?.Invoke(m_list_value_of_question_mark);

      //쿼리문 실행
      if (m_str_query[0] == 'S')
        execute_select_query();
      else
        execute_query();

      // DB 통로 닫기
      disconnect_DB();
      _Init();
    }
    private void remove_single_quote()
    {
      for (int i = 1; i < m_list_value_of_question_mark.Count; ++i)
        m_list_value_of_question_mark[i] = m_list_value_of_question_mark[i].Replace("'", "''");
    }
    private void connect_DB()
    {
      if (m_db_name == null)
        m_connectionPath = $"SERVER={DB_HOST} DATABASE={DB_NAME} UID= {DB_USER} PASSWORD= {DB_PASS} CharSet={ CHAR_SET}";
      else
        m_connectionPath = $"SERVER={DB_HOST} DATABASE={m_db_name} UID= {DB_USER} PASSWORD= {m_db_pass} CharSet = utf8;";

      try
      {
        m_mySqlConnection = new MySqlConnection(m_connectionPath);
        m_mySqlConnection.Open();
      }
      catch (MySqlException _mse)
      {
        switch (_mse.Number)
        {
          case 0:
            Debug.WriteLine("DB연결 실패");
            break;
          case 1045:
            Debug.WriteLine("DB_USER, DB_PASS 중 하나 실패");
            break;
        }
      }
    }
    private void execute_query()
    {
      if (m_str_query.Equals(""))
        return;
      m_mySqlCommand = new MySqlCommand(m_str_query, m_mySqlConnection);
      m_mySqlDataReader = m_mySqlCommand.ExecuteReader();
    }
    private void execute_select_query()
    {
      execute_query();

      m_list_result_receiver = new List<string>[m_column_cnt];
      for (int index = 0; index < m_list_result_receiver.Length; ++index)
        m_list_result_receiver[index] = new List<string>();

      while (m_mySqlDataReader.Read())
      {
        for (int i = 0; i < m_column_cnt; ++i)
          m_list_result_receiver[i].Add(m_mySqlDataReader[i].ToString());
      }
    }
    private void disconnect_DB()
    {
      try
      {
        m_mySqlConnection.Close();
      }
      catch (MySqlException e)
      {
        Debug.WriteLine(e.Message);
      }
    }
    public void set_query(string _str_query) => m_list_value_of_question_mark.Insert(0, _str_query); 
    public void set_value_of_question_mark(string _value_of_question_mark) =>  m_list_value_of_question_mark.Add(_value_of_question_mark);
    public void set_value_of_question_mark<T>(T _value_of_question_mark) => m_list_value_of_question_mark.Add(_value_of_question_mark?.ToString());
    /*public void set_value_of_question_mark(int _value_of_question_mark) => m_list_value_of_question_mark.Add(_value_of_question_mark.ToString()); 
    public void set_value_of_question_mark(double _value_of_question_mark) => m_list_value_of_question_mark.Add(_value_of_question_mark.ToString()); */

    public List<string>[] get_result() => m_list_result_receiver; 
    public int get_row_cnt() => m_list_result_receiver[0].Count; 
    public int get_column_cnt() => m_list_result_receiver.Length; 

    public void Dispose()
    {
      m_mySqlConnection?.Dispose();
      m_mySqlCommand?.Dispose();
      m_mySqlDataReader?.Dispose();
      m_map_query_setter?.GetEnumerator().Dispose();
      GC.SuppressFinalize(true);
    }
  }
}
