using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._02.DTO._02.TheWord;

namespace TwoMites_Engine._01.DAO._02.CTheWord_DAO
{
  public class CTheWord_DAO : DAO_MySQL_v2
  {
    public CTheWord_DAO() { }
    ~CTheWord_DAO() { Dispose(); }

    private DAO_MySQL_v2 dao;
    private const int DEFAULT_CAP = 1024;

    #region 멤버함수 선언부
    public bool InsertTheWord(TheWordDTO? focus_the_word_item) => insert_the_word(focus_the_word_item);
    public List<TheWordDTO>? SelectListTheWord() => select_list_the_word();
    public bool UpdateTheWord(TheWordDTO? focus_the_word_item) => update_the_word(focus_the_word_item);
    public bool DeleteTheWord(int _tw_pk_id) => delete_the_word(_tw_pk_id);
    #endregion

    #region 멤버함수 정의부
    private bool insert_the_word(TheWordDTO? focus_the_word_item)
    {
      using (dao = new DAO_MySQL_v2())
      {
        StringBuilder str_buil = new StringBuilder(1024);
        str_buil.Append("INSERT INTO twomites.THE_WORD ");
        str_buil.Append("(tw_wt_key, tw_pastor, tw_date, tw_created_at, tw_modified_at, tw_title, tw_the_word) ");
        str_buil.Append($" VALUES('{focus_the_word_item?.tw_wt_key}', '{focus_the_word_item?.tw_pastor}' ");
        str_buil.Append($", '{focus_the_word_item?.tw_date.ToString("yyyy/MM/dd")}', DEFAULT, DEFAULT ");
        str_buil.Append($", '{focus_the_word_item?.tw_title}', '{focus_the_word_item?.tw_the_word}'); ");
        dao.Execute(str_buil.ToString());
        return true;
      }
    }

    private List<TheWordDTO>? select_list_the_word()
    {
      List<TheWordDTO> list_the_word = new List<TheWordDTO>();
      using (dao = new DAO_MySQL_v2())
      {
        using (var data_table = dao.GetDataTable("SELECT * FROM  twomites.THE_WORD; "))
        {

          for (int i = 0; i < data_table?.Rows.Count; i++)
          {
            list_the_word.Add(new TheWordDTO(
              int.Parse(data_table.Rows[i]["tw_pk_id"].ToString())
              , int.Parse(data_table.Rows[i]["tw_wt_key"].ToString())
              , data_table.Rows[i]["tw_pastor"].ToString()
              , DateTime.Parse(data_table.Rows[i]["tw_date"].ToString())
              , DateTime.Parse(data_table.Rows[i]["tw_created_at"].ToString())
              , DateTime.Parse(data_table.Rows[i]["tw_modified_at"].ToString())
              , data_table.Rows[i]["tw_title"].ToString()
              , data_table.Rows[i]["tw_the_word"].ToString()
              ));
          }
        }
      }

      return list_the_word;
    }


    private bool update_the_word(TheWordDTO? focus_the_word_item)
    {
      using (dao = new DAO_MySQL_v2())
      {
        StringBuilder str_buil = new StringBuilder(1024);
        str_buil.Append("UPDATE twomites.THE_WORD ");
        str_buil.Append($"SET tw_title = '{focus_the_word_item?.tw_title}'");
        str_buil.Append($", tw_date = '{focus_the_word_item?.tw_date.ToString("yyyy/MM/dd")}'");
        str_buil.Append($", tw_the_word =  '{focus_the_word_item?.tw_the_word}' ");
        str_buil.Append($", tw_pastor =  '{focus_the_word_item?.tw_pastor}' ");
        str_buil.Append(", tw_modified_at = now() ");
        str_buil.Append($" WHERE tw_pk_id = '{focus_the_word_item?.tw_pk_id}'; ");
        dao.Execute(str_buil.ToString());
        return true;
      }
    }

    private bool delete_the_word(int _tw_pk_id)
    {
      using (dao = new DAO_MySQL_v2())
      {
        dao.Execute($"DELETE FROM twomites.THE_WORD WHERE tw_pk_id = {_tw_pk_id};");
        return true;
      }
    }

      #endregion

    }
  }
