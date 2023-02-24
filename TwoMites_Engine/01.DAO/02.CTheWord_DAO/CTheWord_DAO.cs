using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._02.DTO._02.TheWord;

namespace TwoMites_Engine._01.DAO._02.CTheWord_DAO
{
    public class CTheWord_DAO : Engine._01.DAO.MySQL_DAO_v3
    {
        public CTheWord_DAO() { }
        ~CTheWord_DAO() { Dispose(); }

        private Engine._01.DAO.MySQL_DAO_v3 dao;
        private const int DEFAULT_CAP = 1024;

        #region 멤버함수 선언부
        public bool InsertTheWord(TheWordDTO? _focusTheWordItem) => _InsertTheWord(_focusTheWordItem);
        public List<TheWordDTO>? SelectListTheWord() => _SelectListTheWord();
        public bool UpdateTheWord(TheWordDTO? _focusTheWordItem) => _UpdateTheWord(_focusTheWordItem);
        public bool DeleteTheWord(int _tw_pk_id) => _DeleteTheWord(_tw_pk_id);
        #endregion

        #region 멤버함수 정의부
        private bool _InsertTheWord(TheWordDTO? _focusTheWordItem)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                StringBuilder str_buil = new StringBuilder(1024);
                str_buil.Append("INSERT INTO twomites.THE_WORD ");
                str_buil.Append("(tw_wt_key, tw_pastor, tw_date, tw_created_at, tw_modified_at, tw_title, tw_the_word) ");
                str_buil.Append($" VALUES('{_focusTheWordItem?.tw_wt_key}', '{_focusTheWordItem?.tw_pastor}' ");
                str_buil.Append($", '{_focusTheWordItem?.tw_date.ToString("yyyy/MM/dd")}', DEFAULT, DEFAULT ");
                str_buil.Append($", '{_focusTheWordItem?.tw_title}', '{_focusTheWordItem?.tw_the_word}'); ");
                dao.Execute(str_buil.ToString());
                return true;
            }
        }

        private List<TheWordDTO>? _SelectListTheWord()
        {
            List<TheWordDTO> list_the_word = new List<TheWordDTO>();
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable("SELECT * FROM  twomites.THE_WORD; "))
                {

                    for (int i = 0; i < dataTable?.Rows.Count; i++)
                    {
                        list_the_word.Add(new TheWordDTO(
                          int.Parse(dataTable.Rows[i]["tw_pk_id"].ToString())
                          , int.Parse(dataTable.Rows[i]["tw_wt_key"].ToString())
                          , dataTable.Rows[i]["tw_pastor"].ToString()
                          , DateTime.Parse(dataTable.Rows[i]["tw_date"].ToString())
                          , DateTime.Parse(dataTable.Rows[i]["tw_created_at"].ToString())
                          , DateTime.Parse(dataTable.Rows[i]["tw_modified_at"].ToString())
                          , dataTable.Rows[i]["tw_title"].ToString()
                          , dataTable.Rows[i]["tw_the_word"].ToString()
                          ));
                    }
                }
            }

            return list_the_word;
        }


        private bool _UpdateTheWord(TheWordDTO? _focusTheWordItem)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                StringBuilder str_buil = new StringBuilder(1024);
                str_buil.Append("UPDATE twomites.THE_WORD ");
                str_buil.Append($"SET tw_title = '{_focusTheWordItem?.tw_title}'");
                str_buil.Append($", tw_date = '{_focusTheWordItem?.tw_date.ToString("yyyy/MM/dd")}'");
                str_buil.Append($", tw_the_word =  '{_focusTheWordItem?.tw_the_word}' ");
                str_buil.Append($", tw_pastor =  '{_focusTheWordItem?.tw_pastor}' ");
                str_buil.Append(", tw_modified_at = now() ");
                str_buil.Append($" WHERE tw_pk_id = '{_focusTheWordItem?.tw_pk_id}'; ");
                dao.Execute(str_buil.ToString());
                return true;
            }
        }

        private bool _DeleteTheWord(int _tw_pk_id)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"DELETE FROM twomites.THE_WORD WHERE tw_pk_id = {_tw_pk_id};");
                return true;
            }
        }

        #endregion

    }
}
