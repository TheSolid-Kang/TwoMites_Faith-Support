using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwoMites_Engine._01.DAO;
using TwoMites_Engine._02.DTO._01.BibleDTO;
using TwoMites_Engine._99.Headers;

namespace TwoMites._01.Page._00.Bible
{

  public class BibleMainViewModel : ViewModelBase
  {
    public BibleMainViewModel()
    {
      bind_list_dto_bible = new ObservableCollection<DTO_BIBLE>();
      bind_list_testament_name = new ObservableCollection<Bible_Title>();
      focus_bible_item = new DTO_BIBLE("창", "01", "01");
      bind_list_dto_bible = new ObservableCollection<DTO_BIBLE>();
      bind_list_dto_bible_summary = new ObservableCollection<DTO_BIBLE_SUMMARY>();
      bind_list_dto_bible_contemplation = new ObservableCollection<DTO_BIBLE_CONTEMPLATION>();
    }
    ~BibleMainViewModel()
    {
      base.Dispose();
    }


    #region bind 변수
    public string? bible_summary { get; set; }
    public string? bible_contemplation { get; set; }

    public ObservableCollection<DTO_BIBLE>? bind_list_dto_bible { get; set; }
    public ObservableCollection<DTO_BIBLE_SUMMARY>? bind_list_dto_bible_summary { get; set; }
    public ObservableCollection<DTO_BIBLE_CONTEMPLATION>? bind_list_dto_bible_contemplation { get; set; }

    private DTO_BIBLE _focus_bible_item;
    public DTO_BIBLE focus_bible_item 
    { 
      get { return _focus_bible_item; } 
      set { 
        _focus_bible_item = value;

        //navigate_text = $"{_focus_bible_item?.b_chapter} 장 {_focus_bible_item?.b_verse} 절 : {_focus_bible_item?.b_descript}"; 
        //광긇하면 터지는 부분
        //임시방편으로 만듦
        update_list_dto_bible_summary.Execute(null);
        update_list_dto_bible_contemplation.Execute(null);

        NotifyPropertyChanged(nameof(focus_bible_item)); 
      } 
    }

    private Bible_Title _focus_testament_item;
    public Bible_Title focus_testament_item
    {
      get { return _focus_testament_item; }
      set
      {
        _focus_testament_item = value;

        update_list_dto_bible?.Execute(null);

        NotifyPropertyChanged(nameof(focus_testament_item));
      }
    }
    private ObservableCollection<Bible_Title> _bind_list_testament_name;
    public ObservableCollection<Bible_Title> bind_list_testament_name
    { 
      get { return _bind_list_testament_name; } 
      set { _bind_list_testament_name = value;
        using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
        {
          dao.set_query("SELECT * FROM twomites.BIBLE_TITLE");
          dao.set_column_cnt(3);
          dao.execute(QUERY.NORMAL);
          var size = dao.get_row_cnt();
          var list_result = dao.get_result();
          _bind_list_testament_name = new ObservableCollection<Bible_Title>();
          for (int i = 0; i < size; ++i)
          {
            var dto = new Bible_Title(list_result[0][i], list_result[1][i], list_result[2][i]);
            _bind_list_testament_name.Add(dto);
          }
        }
        NotifyPropertyChanged(nameof(bind_list_testament_name)); 
      }
    }
    #endregion


    #region Command
    public ICommand update_list_testament_name => new CDelegateCommand((object _obj) =>
    {
      bind_list_dto_bible?.Clear();
      using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
      {
        dao.set_query("SELECT * FROM twomites.BIBLE_TITLE");
        dao.set_column_cnt(3);
        dao.execute(QUERY.NORMAL);
        var size = dao.get_row_cnt();
        var list_result = dao.get_result();

        for (int i = 0; i < size; ++i)
          _bind_list_testament_name.Add(new Bible_Title(list_result[0][i], list_result[1][i], list_result[2][i]));
      }
      NotifyPropertyChanged(nameof(bind_list_testament_name));
    });

    //권에 맞는 성경 출력
    public ICommand update_list_dto_bible => new CDelegateCommand( (object _obj) => 
      {
        bind_list_dto_bible?.Clear();
        using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
        {
          dao.set_query($"SELECT * FROM twomites.BIBLE WHERE b_book = '{focus_testament_item.bt_name_key}'");
          dao.set_column_cnt(6);
          dao.execute(QUERY.NORMAL);
          var size = dao.get_row_cnt();
          var list_result = dao.get_result();

          Func<string, string> apply_new_line = (string _str_bible) => {
            StringBuilder str_buil = new StringBuilder(512);
            const int DEFAULT_NEW_LINE_INDEX = 35;
            int i = 0;
            for (; (i + 1) * DEFAULT_NEW_LINE_INDEX < _str_bible.Length; ++i)
            {
              int cur_index = i * DEFAULT_NEW_LINE_INDEX;
              _str_bible = _str_bible.Insert((i + 1) * DEFAULT_NEW_LINE_INDEX, "\n");
              str_buil.Append(_str_bible.Substring(cur_index, DEFAULT_NEW_LINE_INDEX)  );
            }
            str_buil.Append(_str_bible.Substring(i * DEFAULT_NEW_LINE_INDEX));

            return str_buil.ToString(); }; 

          for (int i = 0; i < size; ++i)
            bind_list_dto_bible?.Add(new DTO_BIBLE(list_result[0][i], list_result[1][i], list_result[2][i]
              , list_result[3][i], apply_new_line(list_result[4][i]), list_result[5][i]));

          
        }

        NotifyPropertyChanged(nameof(bind_list_dto_bible));

      } );

    //성경 줄거리 목록 출력
    public ICommand update_list_dto_bible_summary => new CDelegateCommand( (object _obj) => 
      {
        bind_list_dto_bible_summary?.Clear();
        using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
        {
          dao.set_query($"SELECT * FROM twomites.BIBLE_SUMMARY WHERE bs_book = '{focus_bible_item?.b_book}' AND bs_chapter = '{focus_bible_item?.b_chapter}' AND bs_verse = '{focus_bible_item?.b_verse}' ");
          dao.set_column_cnt(5);
          dao.execute(QUERY.NORMAL);
          var size = dao.get_row_cnt();
          var list_result = dao.get_result();

          for (int i = 0; i < size; ++i)
            bind_list_dto_bible_summary?.Add(new DTO_BIBLE_SUMMARY(DateTime.Parse(list_result[0][i]), list_result[1][i]
              , list_result[2][i], list_result[3][i], list_result[4][i]));

        }

        NotifyPropertyChanged(nameof(bind_list_dto_bible_summary));
        bible_summary = "";
        NotifyPropertyChanged(nameof(bible_summary));
      });
    //성경 묵상 목록 출력
    public ICommand update_list_dto_bible_contemplation => new CDelegateCommand( (object _obj) => 
      {
        bind_list_dto_bible_contemplation?.Clear();
        using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
        {
          dao.set_query($"SELECT * FROM twomites.BIBLE_CONTEMPLATION WHERE bc_book = '{focus_bible_item.b_book}' AND bc_chapter = '{focus_bible_item.b_chapter}' AND bc_verse = '{focus_bible_item.b_verse}' ");
          dao.set_column_cnt(5);
          dao.execute(QUERY.NORMAL);
          var size = dao.get_row_cnt();
          var list_result = dao.get_result();

          for (int i = 0; i < size; ++i)
            bind_list_dto_bible_contemplation?.Add(new DTO_BIBLE_CONTEMPLATION(DateTime.Parse(list_result[0][i]), list_result[1][i]
              , list_result[2][i], list_result[3][i], list_result[4][i]));
        }

        NotifyPropertyChanged(nameof(bind_list_dto_bible_contemplation));
        bible_contemplation = "";
        NotifyPropertyChanged(nameof(bible_contemplation));
      });


    //성경 줄거리 입력
    public ICommand insert_bible_summary => new CDelegateCommand((object _obj) => 
    { 
      bind_list_dto_bible_summary = new ObservableCollection<DTO_BIBLE_SUMMARY>();
      using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
      {
        dao.set_query($"INSERT INTO twomites.BIBLE_SUMMARY VALUES(now(), '{focus_bible_item.b_book}', '{focus_bible_item.b_chapter}','{focus_bible_item.b_verse}','{bible_summary}')");
        dao.execute(QUERY.NORMAL);
        update_list_dto_bible_summary.Execute(null);
      }
      bible_summary = "";
      NotifyPropertyChanged(nameof(bible_summary));
    });
    //성경 묵상 입력
    public ICommand insert_bible_contemplation => new CDelegateCommand((object _obj) => 
    {
      bind_list_dto_bible_contemplation = new ObservableCollection<DTO_BIBLE_CONTEMPLATION>();
      using (DAO_MySQL dao = m_engine_two_mites.m_bible_mgr.m_dao_mysql)
      {
        dao.set_query($"INSERT INTO twomites.BIBLE_CONTEMPLATION VALUES(now(), '{focus_bible_item?.b_book}', '{focus_bible_item?.b_chapter}','{focus_bible_item?.b_verse}','{bible_contemplation}')");
        dao.execute(QUERY.NORMAL);
        update_list_dto_bible_contemplation.Execute(null);
      }
      bible_contemplation = "";
      NotifyPropertyChanged(nameof(bible_contemplation));
    });
    
    //220618_tk_ 사이드 메뉴 
    public ObservableCollection<CCommander> ListCommander
    {
      get
      {
        ObservableCollection<CCommander> list_commander = new ObservableCollection<CCommander>();
        list_commander.Add(new CCommander("까꿍", new CDelegateCommand((object _obj) => { MessageBox.Show("까꿍이"); }))); //까꿍이
        list_commander.Add(new CCommander());     //empty 커맨드 안내
        return list_commander;
      }
    }

    #endregion

  }
}
