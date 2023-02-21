using Engine._03.CFTPMgr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TwoMites_Engine._02.DTO._03.Fellowship;
using TwoMites_Engine._03.Mgr;
using TwoMites_Engine._99.Headers;

namespace TwoMites._01.Page._03.Fellowship
{
  public class FellowshipMainViewModel : ViewModelBase
  {
    public FellowshipMainViewModel()
    {
      thumb_img_1 = null;
      thumb_img_2 = null;
      list_department = CFellowshipMgr.m_pInstance.SelectListFellowshipDepartment();
    }
    ~FellowshipMainViewModel() { }

    #region 파인딩용
    private ImageSource? _thumb_img_1;
    private ImageSource? _thumb_img_2;
    private ImageSource? _ori_img;

    private ObservableCollection<FellowshipDepartmentDto>? _list_department;
    private string _cur_f_department;

    private string _cur_gender;
    #endregion
    #region property
    public ImageSource? thumb_img_1
    {
      get => _thumb_img_1;
      set
      {
        _thumb_img_1 = value;
        BitmapImage img = new BitmapImage();
        using (FileStream file_stream = File.OpenRead(@"C:\Users\hoppi\Documents\카카오톡 받은 파일\2022_소망부\KakaoTalk_20220607_084212037.jpg"))
        {
          img.BeginInit();
          img.CacheOption = BitmapCacheOption.OnLoad;
          img.StreamSource = file_stream;
          img.EndInit();
          img.Freeze();
          file_stream.Dispose();
          file_stream.Close();
        }
        _thumb_img_1 = img;
        NotifyPropertyChanged(nameof(thumb_img_1));
      }
    }
    public ImageSource? thumb_img_2
    {
      get => _thumb_img_2;
      set
      {
        _thumb_img_2 = value;
        BitmapImage img = new BitmapImage();
        using (FileStream file_stream = File.OpenRead(@"C:\Users\hoppi\Documents\카카오톡 받은 파일\2022_소망부\KakaoTalk_20220607_084205403_01.jpg"))
        {
          img.BeginInit();
          img.CacheOption = BitmapCacheOption.OnLoad;
          img.StreamSource = file_stream;
          img.EndInit();
          img.Freeze();
          file_stream.Dispose();
          file_stream.Close();
        }
        _thumb_img_2 = img;
        NotifyPropertyChanged(nameof(thumb_img_2));
      }
    }
    public ImageSource? ori_img
    {
      get => _ori_img;
      set
      {
        _ori_img = value;
        NotifyPropertyChanged(nameof(ori_img));
      }
    }

    public ObservableCollection<FellowshipDepartmentDto>? list_department
    {
      get => _list_department;
      set
      {
        _list_department = value;
        NotifyPropertyChanged(nameof(list_department));
      }
    }
    public string cur_f_department
    {
      get => _cur_f_department;
      set
      {
        _cur_f_department = value;
        NotifyPropertyChanged(nameof(cur_f_department));
      }
    }

    public string cur_gender
    {
      get => _cur_gender;
      set
      {
        _cur_gender = value;
        NotifyPropertyChanged(nameof(cur_gender));
      }
    }
    #endregion

    #region 날짜선택
    // 기본값을 설정해두지 않으면 0001년으로 표시되기 때문에 기본값을 지정해 주어야 합니다.
    private DateTime _selectedDateTime = DateTime.Now;
    private DateTime _SelectedDate = DateTime.Now;
    public DateTime SelectedDateTime
    {
      get => _selectedDateTime;
      set
      {
        _selectedDateTime = value;
        NotifyPropertyChanged(nameof(SelectedDateTime));
      }
    }
    public DateTime SelectedDate
    {
      get => _SelectedDate;
      set
      {
        _SelectedDate = value;
        SelectedDateTime = value;
        NotifyPropertyChanged(nameof(SelectedDate));
      }
    }
    #endregion


    #region btn command
    //220618_tk_ 사이드 메뉴 
    public ObservableCollection<CCommander> ListCommander
    {
      get
      {
        ObservableCollection<CCommander> obsCommander = new ObservableCollection<CCommander>();
        obsCommander.Add(new CCommander("까꿍", new CDelegateCommand((object _obj) => { MessageBox.Show("까꿍이"); }))); //까꿍이
        obsCommander.Add(new CCommander());     //empty 커맨드 안내
        obsCommander.Add(new CCommander("새등록", new CDelegateCommand((object _obj) => {
          //새로운 교제 등록
          //1. 교제 인스턴스 생성
          FellowshipDto fellowship = new();


          //2. 

          //3. 
        })));     //empty 커맨드 안내
        return obsCommander;
      }
    }

    public ICommand SelectThumbPhoto1 => new CDelegateCommand((object _obj) => ori_img = thumb_img_1);
    public ICommand SelectThumbPhoto2 => new CDelegateCommand((object _obj) => ori_img = thumb_img_2);

    //사진 등록
    public ICommand UploadPhoto => new CDelegateCommand((object _obj) =>
    {
      using (var ftp_mgr = CFTPMgr.get_instance())
      {
        var list_path = ftp_mgr.OpenDlg();

        list_path?.ForEach(x =>
        {
          MessageBox.Show(x);
          //db 저장

          //server에 ori파일 저장

          //server에 thumbnail 저장

        });

      }
    });

    public ICommand CreateFellowship => new CDelegateCommand((object _obj) =>
    {
      //새로운 교제 등록
      //1. 

      //2. 

      //3. 
    });
    public ICommand SaveFellowship => new CDelegateCommand((object _obj) =>
    {
      //현재 교제 저장
      
      //1. 

      //2. 

      //3. 

    });
    public ICommand CancelFellowship => new CDelegateCommand((object _obj) =>
    {
      //1. 

      //2. 

      //3. 

    });


    #endregion

    public override void Dispose()
    {
      base.Dispose();
      _thumb_img_1?.Freeze();
      _thumb_img_1 = null;
      _thumb_img_2?.Freeze();
      _thumb_img_2 = null;
      _ori_img?.Freeze();
      _ori_img = null;
    }




  }
}
