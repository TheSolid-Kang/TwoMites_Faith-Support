using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TwoMites_Engine._01.DAO;
using TwoMites_Engine._02.DTO._02.TheWord;
using TwoMites_Engine._99.Headers;
using TwoMites_Engine._03.Mgr;
using System.ComponentModel;
using System.Windows;
using TwoMites_Engine._01.DAO._02.CTheWord_DAO;

namespace TwoMites._01.Page._01.TheWord
{
  public class TheWordViewModel : ViewModelBase
  {
    public TheWordViewModel()
    {
      _bind_list_dto_the_word = new ObservableCollection<TheWordDTO>();
      _focus_the_word_item = new TheWordDTO();
      _focus_the_word_item.tw_wt_key = 1; // 1 == 주일말씀
      refresh_the_word_list();
    }

    #region bind 변수
    public ObservableCollection<TheWordDTO> _bind_list_dto_the_word;
    public ObservableCollection<TheWordDTO>? bind_list_dto_the_word 
    { 
      get => _bind_list_dto_the_word;
      set 
      { 
        _bind_list_dto_the_word = value; 
        NotifyPropertyChanged(nameof(bind_list_dto_the_word)); 
      } 
    }
    private TheWordDTO _focus_the_word_item;
    public TheWordDTO? focus_the_word_item
    {
      get { return _focus_the_word_item; }
      set
      {
        _focus_the_word_item = value; //V
        NotifyPropertyChanged(nameof(focus_the_word_item));
      }
    }

    #endregion

    #region Command
    //220618_tk_ 사이드 메뉴 
    public ObservableCollection<CCommander> ListCommander
    {
      get
      {
        ObservableCollection<CCommander> list_commander = new ObservableCollection<CCommander>();
        list_commander.Add(new CCommander("새등록", CreateTheWord));
        list_commander.Add(new CCommander("저장", SaveTheWord)); 
        list_commander.Add(new CCommander("삭제", DeleteTheWord));  
        return list_commander;
      }
    }
    public ICommand CreateTheWord => new CDelegateCommand((object _obj) =>
    {
      _focus_the_word_item = new TheWordDTO();
      _focus_the_word_item.tw_wt_key = 1; // 1 == 주일말씀
      using (var dao = new CTheWord_DAO())
        dao.InsertTheWord(focus_the_word_item);

      refresh_the_word_list();
      focus_the_word_item = _bind_list_dto_the_word.Reverse().ToArray()[0];
    });
    public ICommand SaveTheWord => new CDelegateCommand((object _obj) =>
    {
      int tw_pk_id = _focus_the_word_item.tw_pk_id;
      using (var dao = new CTheWord_DAO())
        dao.UpdateTheWord(focus_the_word_item);

      refresh_the_word_list();
      //220710_tk 왜 안 돼는지 모름 일단 보류
      //focus_the_word_item = _bind_list_dto_the_word.Where(element => element.tw_pk_id == tw_pk_id).Select(element => new TheWordDTO(element)) as TheWordDTO;
      foreach(var element in _bind_list_dto_the_word)
        if (element.tw_pk_id == tw_pk_id)
          focus_the_word_item = element;
    });
    public ICommand DeleteTheWord => new CDelegateCommand((object _obj) => {
      using (var dao = new CTheWord_DAO())
        dao.DeleteTheWord(_focus_the_word_item != null ? _focus_the_word_item.tw_pk_id : 0);
      refresh_the_word_list();
      if(_bind_list_dto_the_word.Count > 0)
       focus_the_word_item = bind_list_dto_the_word?.ElementAt(0);
    });
    public ICommand CancelWriteTheWord => new CDelegateCommand((object _obj) =>
    {

    });
    #endregion
    
    #region
    private bool refresh_the_word_list()
    {
      bind_list_dto_the_word?.Clear();
      bind_list_dto_the_word = null;
      using (var dao = new CTheWord_DAO())
      {
        var list_the_word = dao.SelectListTheWord();
        //220710_tk list를 날짜별로 정렬헤서 ObservableCollection 만듦
        bind_list_dto_the_word = new ObservableCollection<TheWordDTO>(list_the_word?.OrderBy(element => element?.tw_date));
      }
      return _bind_list_dto_the_word != null ? true: false;
    }


    #endregion

  }
}
