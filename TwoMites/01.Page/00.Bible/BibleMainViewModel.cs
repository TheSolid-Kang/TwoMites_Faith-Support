using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwoMites_Engine._01.DAO;
using TwoMites_Engine._01.DAO._01.CBible_DAO;
using TwoMites_Engine._02.DTO._01.BibleDTO;
using TwoMites_Engine._02.DTO._02.TheWord;
using TwoMites_Engine._99.Headers;

namespace TwoMites._01.Page._00.Bible
{

    public class BibleMainViewModel : ViewModelBase
    {
        public BibleMainViewModel()
        {
            LV_ListBibleDto = new ObservableCollection<BibleDto>();
            LV_ListTestamentName = new ObservableCollection<BibleTitleDto>();
            //LV_FocusBibleItem = new BibleDto("창", "1", "1");
            LV_FocusBibleItem = new BibleDto();
            LV_ListBibleDto = new ObservableCollection<BibleDto>();
            LV_ListBibleSummary = new ObservableCollection<BibleSummaryDto>();
            LV_ListBibleContemplation = new ObservableCollection<BibleContemplationDto>();
        }
        ~BibleMainViewModel()
        {
            base.Dispose();
        }


        #region bind 변수
        public string? TB_BibleSummary { get; set; }
        public string? TB_BibleContemplation { get; set; }

        public ObservableCollection<BibleDto>? LV_ListBibleDto { get; set; }
        public ObservableCollection<BibleSummaryDto>? LV_ListBibleSummary { get; set; }
        public ObservableCollection<BibleContemplationDto>? LV_ListBibleContemplation { get; set; }

        private BibleDto _LV_FocusBibleItem;
        public BibleDto? LV_FocusBibleItem
        {
            get { return _LV_FocusBibleItem; }
            set
            {
                _LV_FocusBibleItem = value;

                //navigate_text = $"{_LV_FocusBibleItem?.b_chapter} 장 {_LV_FocusBibleItem?.b_verse} 절 : {_LV_FocusBibleItem?.b_descript}"; 
                //광긇하면 터지는 부분
                //임시방편으로 만듦
                UpdateListBibleSummaryDto.Execute(null);
                UpdateListBibleContemplationDto.Execute(null);

                NotifyPropertyChanged(nameof(LV_FocusBibleItem));
            }
        }

        private BibleTitleDto _LV_FocusTestamentItem;
        public BibleTitleDto LV_FocusTestamentItem
        {
            get { return _LV_FocusTestamentItem; }
            set
            {
                _LV_FocusTestamentItem = value;

                UpdateListBibleDto?.Execute(null);

                NotifyPropertyChanged(nameof(LV_FocusTestamentItem));
            }
        }

        private ObservableCollection<BibleTitleDto> _LV_ListTestamentName;
        public ObservableCollection<BibleTitleDto> LV_ListTestamentName
        {
            get { return _LV_ListTestamentName; }
            set
            {
                _LV_ListTestamentName = value;
                using (CBible_DAO dao = new CBible_DAO())
                {
                    _LV_ListTestamentName = dao.SelectBibleTitle();
                }
                NotifyPropertyChanged(nameof(LV_ListTestamentName));
            }
        }
        #endregion


        #region Command
        public ICommand UpdateListTestamentName => new CDelegateCommand((object _obj) =>
        {
            LV_ListBibleDto?.Clear();
            using (CBible_DAO dao = new CBible_DAO())
            {
                _LV_ListTestamentName = dao.SelectBibleTitle();
            }
            NotifyPropertyChanged(nameof(LV_ListTestamentName));
        });

        //권에 맞는 성경 출력
        public ICommand UpdateListBibleDto => new CDelegateCommand((object _obj) =>
          {
              LV_ListBibleDto?.Clear();
              using (CBible_DAO dao = new CBible_DAO())
              {
                  LV_ListBibleDto = dao.SelectBible(LV_FocusTestamentItem.bt_name_key);
                  Func<string, string> applyNewLine = (string _str_bible) =>
                  {
                      StringBuilder str_buil = new StringBuilder(512);
                      const int DEFAULT_NEW_LINE_INDEX = 35;
                      int i = 0;
                      for (; (i + 1) * DEFAULT_NEW_LINE_INDEX < _str_bible.Length; ++i)
                      {
                          int cur_index = i * DEFAULT_NEW_LINE_INDEX;
                          _str_bible = _str_bible.Insert((i + 1) * DEFAULT_NEW_LINE_INDEX, "\n");
                          str_buil.Append(_str_bible.Substring(cur_index, DEFAULT_NEW_LINE_INDEX));
                      }
                      str_buil.Append(_str_bible.Substring(i * DEFAULT_NEW_LINE_INDEX));

                      return str_buil.ToString();
                  };

                  for (int i = 0; i < LV_ListBibleDto.Count; ++i)
                  {
                      LV_ListBibleDto[i].b_descript = applyNewLine(LV_ListBibleDto[i].b_descript);
                  }
              }

              NotifyPropertyChanged(nameof(LV_ListBibleDto));

          });

        //성경 줄거리 목록 출력
        public ICommand UpdateListBibleSummaryDto => new CDelegateCommand((object _obj) =>
          {
              LV_ListBibleSummary?.Clear();
              using (CBible_DAO dao = new CBible_DAO())
              {
                  LV_ListBibleSummary = dao.SelectBibleSummary(LV_FocusBibleItem?.b_book, LV_FocusBibleItem?.b_chapter, LV_FocusBibleItem?.b_verse);
              }


              NotifyPropertyChanged(nameof(LV_ListBibleSummary));
              TB_BibleSummary = "";
              NotifyPropertyChanged(nameof(TB_BibleSummary));
          });
        //성경 묵상 목록 출력
        public ICommand UpdateListBibleContemplationDto => new CDelegateCommand((object _obj) =>
          {
              LV_ListBibleContemplation?.Clear();
              using (CBible_DAO dao = new CBible_DAO())
              {
                  LV_ListBibleContemplation = dao.SelectBibleContemplation(LV_FocusBibleItem?.b_book, LV_FocusBibleItem?.b_chapter, LV_FocusBibleItem?.b_verse);
              }
              NotifyPropertyChanged(nameof(LV_ListBibleContemplation));
              TB_BibleContemplation = "";
              NotifyPropertyChanged(nameof(TB_BibleContemplation));
          });


        //성경 줄거리 입력
        public ICommand InsertBibleSummary => new CDelegateCommand((object _obj) =>
        {
            LV_ListBibleSummary = new ObservableCollection<BibleSummaryDto>();
            using (CBible_DAO dao = new CBible_DAO())
            {
                dao.InsertBibleSummary(LV_FocusBibleItem.b_book, LV_FocusBibleItem.b_chapter, LV_FocusBibleItem.b_verse, TB_BibleSummary);
            }
            TB_BibleSummary = "";
            NotifyPropertyChanged(nameof(TB_BibleSummary));
            UpdateListBibleSummaryDto?.Execute(null);
        });
        //성경 묵상 입력
        public ICommand InsertBibleContemplation => new CDelegateCommand((object _obj) =>
        {
            LV_ListBibleContemplation = new ObservableCollection<BibleContemplationDto>();
            using (CBible_DAO dao = new CBible_DAO())
            {
                dao.InsertBibleContemplation(LV_FocusBibleItem.b_book, LV_FocusBibleItem.b_chapter, LV_FocusBibleItem.b_verse, TB_BibleContemplation);
            }
            TB_BibleContemplation = "";
            NotifyPropertyChanged(nameof(TB_BibleContemplation));
            UpdateListBibleContemplationDto?.Execute(null);
        });

        //220618_tk_ 사이드 메뉴 
        public ObservableCollection<CCommander> ListCommander
        {
            get
            {
                ObservableCollection<CCommander> obsCommander = new ObservableCollection<CCommander>();
                obsCommander.Add(new CCommander("까꿍", new CDelegateCommand((object _obj) => { MessageBox.Show("까꿍이"); }))); //까꿍이
                obsCommander.Add(new CCommander());     //empty 커맨드 안내
                return obsCommander;
            }
        }

        #endregion
        #region search bind 변수
        public DateTime DP_SearchFromDate { get; set; }
        public DateTime DP_SearchToDate { get; set; }
        public bool CB_IsSearchAll { get; set; } = true;

        public string TB_SearchKeyword { get; set; }
        public ICommand Search => new CDelegateCommand((object _obj) =>
        {
            LV_ListBibleDto?.Clear();
            char[] chDelimiter = { ',' };
            List<string> listSearchKeword = TB_SearchKeyword.Split(chDelimiter).ToList();
            for(int i = 0; i < listSearchKeword.Count; ++i)
                listSearchKeword[i] = listSearchKeword[i].Trim();

            using (CBible_DAO dao = new CBible_DAO())
            {
                LV_ListBibleDto = dao.SelectSearchedBible(listSearchKeword, CB_IsSearchAll);
            }

            NotifyPropertyChanged(nameof(LV_ListBibleDto));
        });
        #endregion
    }
}
