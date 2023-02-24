using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwoMites_Engine._02.DTO._01.BibleDTO;
using TwoMites_Engine._02.DTO._02.TheWord;

namespace TwoMites_Engine._01.DAO._01.CBible_DAO
{
    public class CBible_DAO : Engine._01.DAO.MySQL_DAO_v3
    {
        public CBible_DAO() { }
        ~CBible_DAO() { Dispose(); }

        private Engine._01.DAO.MySQL_DAO_v3 dao;
        private const int DEFAULT_CAP = 1024;


        #region 멤버함수 선언부
        public ObservableCollection<BibleTitleDto> SelectBibleTitle() => _SelectBibleTitle();
        public ObservableCollection<BibleDto> SelectBible(string _bt_name_key) => _SelectBible(_bt_name_key);
        public ObservableCollection<BibleSummaryDto> SelectBibleSummary(string _b_book, string _b_chapter, string _b_verse) => _SelectBibleSummary(_b_book, _b_chapter, _b_verse);
        public ObservableCollection<BibleContemplationDto> SelectBibleContemplation(string _b_book, string _b_chapter, string _b_verse) => _SelectBibleContemplation(_b_book, _b_chapter, _b_verse);
        public bool InsertBibleSummary(string _b_book, string _b_chapter, string _b_verse, string _bibleSummary) => _InsertBibleSummary(_b_book, _b_chapter, _b_verse, _bibleSummary);
        public bool InsertBibleContemplation(string _b_book, string _b_chapter, string _b_verse, string _bibleContemplation) => _InsertBibleContemplation(_b_book, _b_chapter, _b_verse, _bibleContemplation);
        #endregion

        #region 멤버함수 정의부
        private ObservableCollection<BibleTitleDto> _SelectBibleTitle()
        {
            var obsBibleTitle = new ObservableCollection<BibleTitleDto>();
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable("SELECT * FROM twomites.BIBLE_TITLE;"))
                {
                    for (int i = 0; i < dataTable.Rows.Count; ++i)
                    {
                        obsBibleTitle.Add(new BibleTitleDto(dataTable.Rows[i]["bt_id"].ToString()
                          , dataTable.Rows[i]["bt_name"].ToString()
                          , dataTable.Rows[i]["bt_name_key"].ToString()));
                    }
                }
            }
            return obsBibleTitle;
        }
        private ObservableCollection<BibleDto> _SelectBible(string _bt_name_key)
        {
            var obsBible = new ObservableCollection<BibleDto>();
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable($"SELECT * FROM twomites.BIBLE WHERE b_book = '{_bt_name_key}';"))
                {
                    for (int i = 0; i < dataTable.Rows.Count; ++i)
                    {
                        obsBible.Add(new BibleDto(dataTable.Rows[i]["b_book"].ToString()
                          , dataTable.Rows[i]["b_chapter"].ToString()
                          , dataTable.Rows[i]["b_verse"].ToString()
                          , dataTable.Rows[i]["b_descript"].ToString()
                          , dataTable.Rows[i]["b_full_descript"].ToString()));
                    }
                }
            }
            return obsBible;
        }
        private ObservableCollection<BibleSummaryDto> _SelectBibleSummary(string _b_book, string _b_chapter, string _b_verse)
        {
            var obsBible = new ObservableCollection<BibleSummaryDto>();
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable($"SELECT * FROM twomites.BIBLE_SUMMARY WHERE bs_book = '{_b_book}' AND bs_chapter = '{_b_chapter}' AND bs_verse = '{_b_verse}'"))
                {
                    for (int i = 0; i < dataTable?.Rows.Count; ++i)
                    {
                        obsBible.Add(new BibleSummaryDto(DateTime.Parse(dataTable?.Rows[i]["bs_date"].ToString())
                          , dataTable.Rows[i]["bs_book"].ToString()
                          , dataTable.Rows[i]["bs_chapter"].ToString()
                          , dataTable.Rows[i]["bs_verse"].ToString()
                          , dataTable.Rows[i]["bs_descript"].ToString()));
                    }
                }
            }
            return obsBible;
        }

        private ObservableCollection<BibleContemplationDto> _SelectBibleContemplation(string _b_book, string _b_chapter, string _b_verse)
        {
            var obsBible = new ObservableCollection<BibleContemplationDto>();
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable($"SELECT * FROM twomites.BIBLE_CONTEMPLATION WHERE bc_book = '{_b_book}' AND bc_chapter = '{_b_chapter}' AND bc_verse = '{_b_verse}';"))
                {
                    for (int i = 0; i < dataTable?.Rows.Count; ++i)
                    {
                        obsBible.Add(new BibleContemplationDto(DateTime.Parse(dataTable.Rows[i]["bc_date"].ToString())
                          , dataTable.Rows[i]["bc_book"].ToString()
                          , dataTable.Rows[i]["bc_chapter"].ToString()
                          , dataTable.Rows[i]["bc_verse"].ToString()
                          , dataTable.Rows[i]["bc_descript"].ToString()));
                    }
                }
            }
            return obsBible;
        }
        private bool _InsertBibleSummary(string _b_book, string _b_chapter, string _b_verse, string _bibleSummary)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"INSERT INTO twomites.BIBLE_SUMMARY VALUES(now(), '{_b_book}', '{_b_chapter}','{_b_verse}','{_bibleSummary}')");
            }
            return true;
        }
        private bool _InsertBibleContemplation(string _b_book, string _b_chapter, string _b_verse, string _bibleContemplation)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"INSERT INTO twomites.BIBLE_CONTEMPLATION VALUES(now(), '{_b_book}', '{_b_chapter}','{_b_verse}','{_bibleContemplation}')");
            }
            return true;
        }

        #endregion

        public void Dispose()
        {
            dao?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
