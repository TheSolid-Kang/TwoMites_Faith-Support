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
        public ObservableCollection<BibleDto> SelectSearchedBible(List<string> _listSearchKeyword, bool _isAnd = true) => _SelectSearchedBible(_listSearchKeyword, _isAnd);
        public ObservableCollection<BibleSummaryDto> SelectBibleSummary(string _b_book, string _b_chapter, string _b_verse) => _SelectBibleSummary(_b_book, _b_chapter, _b_verse);
        public ObservableCollection<BibleContemplationDto> SelectBibleContemplation(string _b_book, string _b_chapter, string _b_verse) => _SelectBibleContemplation(_b_book, _b_chapter, _b_verse);

        public bool InsertBibleSummary(int _b_pk_id, string _b_book, string _b_chapter, string _b_verse, string _bibleSummary) => _InsertBibleSummary(_b_pk_id, _b_book, _b_chapter, _b_verse, _bibleSummary);
        public bool UpdateBibleSummary(int _b_pk_id, string _description) => _UpdateBibleSummary(_b_pk_id, _description);
        public bool DeleteBibleSummary(int _bs_pk_id) => _DeleteBibleSummary(_bs_pk_id);

        public bool InsertBibleContemplation(int _b_pk_id, string _b_book, string _b_chapter, string _b_verse, string _bibleContemplation) => _InsertBibleContemplation(_b_pk_id, _b_book, _b_chapter, _b_verse, _bibleContemplation);
        public bool UpdateBibleContemplation(int _b_pk_id, string _description) => _UpdateBibleContemplation(_b_pk_id,_description);
        public bool DeleteBibleContemplation(int _bc_pk_id) => _DeleteBibleContemplation(_bc_pk_id);
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
                        obsBible.Add(new BibleDto(int.Parse(dataTable.Rows[i]["b_pk_id"].ToString())
                          , dataTable.Rows[i]["b_book"].ToString()
                          , dataTable.Rows[i]["b_chapter"].ToString()
                          , dataTable.Rows[i]["b_verse"].ToString()
                          , dataTable.Rows[i]["b_descript"].ToString()
                          , dataTable.Rows[i]["b_full_descript"].ToString()));
                    }
                }
            }
            return obsBible;
        }
        private ObservableCollection<BibleDto> _SelectSearchedBible(List<string> _listSearchKeyword, bool _isAnd = true)
        {
            List<BibleDto> listBible = null;

            var iterable = from _searchKey in _listSearchKeyword select _searchKey;
            var searchKeywords = _listSearchKeyword.Aggregate((_result, _searchKeyowrd) => _result += ("|" + _searchKeyowrd));

            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable($"SELECT * FROM twomites.BIBLE WHERE b_descript REGEXP '{searchKeywords}';"))
                {
                    if (dataTable == null)
                        return null;

                    listBible = new List<BibleDto>(dataTable.Rows.Count);
                    for (int i = 0; i < dataTable.Rows.Count; ++i)
                    {
                        listBible.Add(new BibleDto(int.Parse(dataTable.Rows[i]["b_pk_id"].ToString())
                          , dataTable.Rows[i]["b_book"].ToString()
                          , dataTable.Rows[i]["b_chapter"].ToString()
                          , dataTable.Rows[i]["b_verse"].ToString()
                          , dataTable.Rows[i]["b_descript"].ToString()
                          , dataTable.Rows[i]["b_full_descript"].ToString()));
                    }
                }
            }
            if (true == _isAnd)//and 조건
            {
                foreach (var _searchKeyword in _listSearchKeyword)
                {
                    var _listBible = (from _bibleDto in listBible
                                      where _bibleDto.b_descript.Contains(_searchKeyword)
                                      select _bibleDto).ToList();
                    listBible = _listBible;
                }
                return new ObservableCollection<BibleDto>(listBible);
            }
            else//or 조건
            {
                return new ObservableCollection<BibleDto>(listBible);
            }
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
                        obsBible.Add(new BibleSummaryDto(int.Parse(dataTable?.Rows[i]["bs_pk_id"].ToString())
                          , int.Parse(dataTable?.Rows[i]["b_fk_id"].ToString())
                          , DateTime.Parse(dataTable?.Rows[i]["bs_date"].ToString())
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
                        obsBible.Add(new BibleContemplationDto(int.Parse(dataTable?.Rows[i]["bc_pk_id"].ToString())
                          , int.Parse(dataTable?.Rows[i]["b_fk_id"].ToString())
                          , DateTime.Parse(dataTable.Rows[i]["bc_date"].ToString())
                          , dataTable.Rows[i]["bc_book"].ToString()
                          , dataTable.Rows[i]["bc_chapter"].ToString()
                          , dataTable.Rows[i]["bc_verse"].ToString()
                          , dataTable.Rows[i]["bc_descript"].ToString()));
                    }
                }
            }
            return obsBible;
        }
        private bool _InsertBibleSummary(int _b_pk_id, string _b_book, string _b_chapter, string _b_verse, string _bibleSummary)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"INSERT INTO twomites.BIBLE_SUMMARY(b_fk_id, bs_date, bs_book, bs_chapter, bs_verse, bs_descript) VALUES('{_b_pk_id}', now(), '{_b_book}', '{_b_chapter}','{_b_verse}','{_bibleSummary}')");
            }
            return true;
        }
        private bool _UpdateBibleSummary(int _b_pk_id, string _description)
        {
            using(var dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"UPDATE BIBLE_SUMMARY SET bs_descript = '{_description}' WHERE b_fk_id = {_b_pk_id}");
            }
            return true;
        }
        private bool _DeleteBibleSummary(int _bs_pk_id)
        {
            using (var dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"DELETE FROM BIBLE_SUMMARY WHERE bs_pk_id = {_bs_pk_id}");
            }
            return true;
        }
        private bool _InsertBibleContemplation(int _b_pk_id, string _b_book, string _b_chapter, string _b_verse, string _bibleContemplation)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"INSERT INTO twomites.BIBLE_CONTEMPLATION(b_fk_id, bc_date, bc_book, bc_chapter, bc_verse, bc_descript) VALUES('{_b_pk_id}', now(), '{_b_book}', '{_b_chapter}','{_b_verse}','{_bibleContemplation}')");
            }
            return true;
        }
        private bool _UpdateBibleContemplation(int _b_pk_id, string _description)
        {
            using (var dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"UPDATE BIBLE_CONTEMPLATION SET bc_descript = '{_description}' WHERE b_fk_id = {_b_pk_id}");
            }
            return true;
        }
        private bool _DeleteBibleContemplation(int _bc_pk_id)
        {
            using (var dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                dao.Execute($"DELETE FROM BIBLE_CONTEMPLATION WHERE bc_pk_id = {_bc_pk_id}");
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
