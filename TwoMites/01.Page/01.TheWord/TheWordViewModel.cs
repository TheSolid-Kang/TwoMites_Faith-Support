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
      _LV_ListTheWordDto = new ObservableCollection<TheWordDTO>();
      _LV_focusTheWordItem = new TheWordDTO();
      _LV_focusTheWordItem.tw_wt_key = 1; // 1 == 주일말씀
      refresh_the_word_list();
    }

    #region bind 변수
    public ObservableCollection<TheWordDTO> _LV_ListTheWordDto;
    public ObservableCollection<TheWordDTO>? LV_ListTheWordDto 
    { 
      get => _LV_ListTheWordDto;
      set 
      { 
        _LV_ListTheWordDto = value; 
        NotifyPropertyChanged(nameof(LV_ListTheWordDto)); 
      } 
    }
    private TheWordDTO _LV_focusTheWordItem;
    public TheWordDTO? LV_focusTheWordItem
    {
      get { return _LV_focusTheWordItem; }
      set
      {
        _LV_focusTheWordItem = value; //V
        NotifyPropertyChanged(nameof(LV_focusTheWordItem));
      }
    }

    #endregion

    #region Command
    //220618_tk_ 사이드 메뉴 
    public ObservableCollection<CCommander> ListCommander
    {
      get
      {
        ObservableCollection<CCommander> obsCommander = new ObservableCollection<CCommander>();
        obsCommander.Add(new CCommander("새등록", CreateTheWord));
        obsCommander.Add(new CCommander("저장", SaveTheWord)); 
        obsCommander.Add(new CCommander("삭제", DeleteTheWord));  
        return obsCommander;
      }
    }
    public ICommand CreateTheWord => new CDelegateCommand((object _obj) =>
    {
      _LV_focusTheWordItem = new TheWordDTO();
      _LV_focusTheWordItem.tw_wt_key = 1; // 1 == 주일말씀
      using (var dao = new CTheWord_DAO())
        dao.InsertTheWord(LV_focusTheWordItem);

      refresh_the_word_list();
      LV_focusTheWordItem = _LV_ListTheWordDto.Reverse().ToArray()[0];
    });
    public ICommand SaveTheWord => new CDelegateCommand((object _obj) =>
    {
      int tw_pk_id = _LV_focusTheWordItem.tw_pk_id;
      using (var dao = new CTheWord_DAO())
        dao.UpdateTheWord(LV_focusTheWordItem);

      refresh_the_word_list();
      //220710_tk 왜 안 돼는지 모름 일단 보류
      //LV_focusTheWordItem = _LV_ListTheWordDto.Where(element => element.tw_pk_id == tw_pk_id).Select(element => new TheWordDTO(element)) as TheWordDTO;
      foreach(var element in _LV_ListTheWordDto)
        if (element.tw_pk_id == tw_pk_id)
          LV_focusTheWordItem = element;
    });
    public ICommand DeleteTheWord => new CDelegateCommand((object _obj) => {
      using (var dao = new CTheWord_DAO())
        dao.DeleteTheWord(_LV_focusTheWordItem != null ? _LV_focusTheWordItem.tw_pk_id : 0);
      refresh_the_word_list();
      if(_LV_ListTheWordDto.Count > 0)
       LV_focusTheWordItem = LV_ListTheWordDto?.ElementAt(0);
    });
    public ICommand CancelWriteTheWord => new CDelegateCommand((object _obj) =>
    {

    });
    #endregion
    
    #region
    private bool refresh_the_word_list()
    {
      LV_ListTheWordDto?.Clear();
      LV_ListTheWordDto = null;
      using (var dao = new CTheWord_DAO())
      {
        var list_the_word = dao.SelectListTheWord();
        //220710_tk list를 날짜별로 정렬헤서 ObservableCollection 만듦
        LV_ListTheWordDto = new ObservableCollection<TheWordDTO>(list_the_word?.OrderBy(element => element?.tw_date));
      }
      return _LV_ListTheWordDto != null ? true: false;
    }


    #endregion
    #region search bind 변수
    public DateTime SearchFromDate { get; set; }
    public DateTime SearchToDate { get; set; }
    public bool IsSearchAll { get; set; } = true;

    public string SearchKeyword { get; set; }
    public ICommand Search => new CDelegateCommand((object _obj) => {
      char[] ch_split = { ',' };
      string[] arr_search_keyword = SearchKeyword.Split(ch_split);


      var list_the_word = (from element in LV_ListTheWordDto
                           where element.tw_the_word.Contains(arr_search_keyword[0].Trim())
                           select element).ToList();
      LV_ListTheWordDto = new ObservableCollection<TheWordDTO>(list_the_word);

    });
    #endregion
  }
}
