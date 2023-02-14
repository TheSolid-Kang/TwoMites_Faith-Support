using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace TwoMites_Engine._03.Mgr
{
  public class FTP_INFO
  {
    public FTP_INFO()
    {
      this.ip_addr = "localhost"; //IP 주소
      this.port = "21"; //FTP 접속 Port
      this.user = ""; //FTP 접속 계정
      this.pwd = ""; //FTP 계정 비밀번호
      this.server_path = @"";
    }
    public FTP_INFO(string ip_addr, string port, string user, string pwd)
    {
      this.ip_addr = ip_addr;
      this.port = port;
      this.user = user;
      this.pwd = pwd;
      server_path = @"www/data/temp";
    }

    public string ip_addr { get; set; }
    public string user { get; set; }
    public string pwd { get; set; }
    public string port { get; set; }

    public string connect_result { get; set; }

    public string server_path { get; set; }
    public string local_path { get; set; }
  }
  public class CFTPMgr : IDisposable
  {
    #region DEFAULT (생성자/ 홀더/ INSTANCE / Destroy)       
    private CFTPMgr() { }
    private static readonly Lazy<CFTPMgr> _p_instance = new Lazy<CFTPMgr>(() => new CFTPMgr());
    public static CFTPMgr get_instance() => _p_instance.Value;
    public void DestroyMgr() => GC.Collect();
    #endregion


    #region FTP 멤버변수
    public delegate void ExceptionEventHandler(string LocationID, Exception ex);
    public event ExceptionEventHandler ExceptionEvent;
    public Exception LastException = null;

    public bool IsConnected { get; set; }
    private FTP_INFO _ftp_info = new FTP_INFO();
    public FTP_INFO ftp_info => _ftp_info;
    #endregion


    #region 멤버함수 선언부
    //1. 서버 연결
    public bool ConnectToServer() => connect_to_server();
    public bool ConnectToServer(string ip, string port, string userId, string pwd) => connect_to_server(ip, port, userId, pwd);

    //2. 서버에 파일 업로드 
    public bool UpLoad(string _server_path, string _local_file_path) => upload(_server_path, _local_file_path);
    //2_2. 서버에 여러 파일 업로드 

    public void MakeDirectory(string _server_path) => make_directory(_server_path);

    //3. 서버에서 파일 다운로드
    public bool DownLoad(string _local_file_path, string _server_path) => download(_local_file_path, _server_path);

    public void CheckDirectory(string _local_file_path) => check_directory(_local_file_path);

    //4. 서버의 해당 폴더에 담긴 파일 리스트 가져옴
    public List<string> GetFileList(string _server_path) => get_file_list(_server_path);

    //5. 파일 다이어로그 출력_여러 파일 선택 가능
    public List<string> OpenDlg() => open_dlg();
    //6. 파일 다이어로그 출력_저장 위치 선택
    public string GetSaveFilePath() => get_save_file_path();

    //7. 서버의 특정 폴더에 저장된 파일 삭제  
    public bool DeleteSavedFileInServer(string _server_file_path) => delete_saved_file_in_server(_server_file_path);
    //7_2. 서버에서 여러 파일 다운로드
    public bool DeleteSavedFileInServer(List<string> _list_server_file_path) => delete_saved_file_in_server(_list_server_file_path);
    //7_3. 서버의 특정 폴더에 저장된 파일 전체 삭제  
    public bool DeleteAllSavedFileInServer(string _server_path) => delete_all_saved_file_in_server(_server_path);
    #endregion


    #region 멤버함수 정의부

    //1. 서버 연결
    private bool connect_to_server() => connect_to_server(_ftp_info.ip_addr, _ftp_info.port, _ftp_info.user, _ftp_info.pwd);
    private bool connect_to_server(string ip, string port, string userId, string pwd)
    {
      this.IsConnected = false;
      this.ftp_info.ip_addr = ip;
      this.ftp_info.port = port;
      this.ftp_info.user = userId;
      this.ftp_info.pwd = pwd;

      string url = string.Format(@"FTP://{0}:{1}/", this.ftp_info.ip_addr, this.ftp_info.port);

      try
      {
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
        ftpRequest.Credentials = new NetworkCredential(userId, pwd);
        ftpRequest.KeepAlive = false;
        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        ftpRequest.UsePassive = false;

        using (ftpRequest.GetResponse())
        {

        }

        this.IsConnected = true;
      }

      catch (Exception ex)
      {
        //Logger._OutputErrorMsg(ex.Message);

        MessageBox.Show($"FTP 접속 실패 \n{ex.Message}");
        this.LastException = ex;
        System.Reflection.MemberInfo info = System.Reflection.MethodInfo.GetCurrentMethod();
        string id = string.Format("{0}.{1}", info.ReflectedType.Name, info.Name);

        if (this.ExceptionEvent != null)
          this.ExceptionEvent(id, ex);

        return false;
      }
      return true;
    }

    //2. 서버에 파일 업로드 
    private bool upload(string _server_path, string _local_file_path)
    {

      try
      {

        _server_path = _server_path.Replace('\\', '/'); //역슬래시 -> 슬래시
        _server_path = _server_path.Replace("//", "/"); //슬래시 -> 슬래시
        _local_file_path = _local_file_path.Replace('\\', '/'); //역슬래시 -> 슬래시
                                                                //make_directory 해당 로직 시간 엄청 오래걸림
                                                                //한 번만 검사하는 거 만들어야 함.
                                                                //make_directory(_server_path); //폴더가 없을 수도 있으니 확인


        FileInfo fileInf = null;
        fileInf = new FileInfo(_local_file_path);

        string url = "";
        if (!_server_path[_server_path.Length - 1].Equals('/'))
          url = string.Format(@"FTP://{0}:{1}/{2}", this.ftp_info.ip_addr, this.ftp_info.port, _server_path);
        else
          url = string.Format(@"FTP://{0}:{1}/{2}{3}", this.ftp_info.ip_addr, this.ftp_info.port, _server_path, fileInf.Name);




        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
        //주소를 바꿔가면서 올릴 방법이 있음.
        //ftprequest에서 주소만 바꾸는 거 찾아야 함.
        ftpRequest.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd); //쓰기 권한이 있는 FTP 사용자 로그인 지정
        ftpRequest.KeepAlive = true;
        ftpRequest.UseBinary = false;
        //ftpRequest.UsePassive = false;
        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile; //FTP 업로드 한다는 것을 표기
        ftpRequest.ContentLength = fileInf.Length;
        int buffLength = 2048;
        byte[] buff = new byte[buffLength]; //입력 파일을 BYTE 배열로 읽음.


        using (FileStream fs = fileInf.OpenRead())
        {
          using (Stream strm = ftpRequest.GetRequestStream())
          {
            int contentLen = fs.Read(buff, 0, buffLength);
            while (contentLen != 0)
            {
              strm.Write(buff, 0, contentLen);
              contentLen = fs.Read(buff, 0, buffLength);
            }
          }
          fs.Flush();
          fs.Close();
        }

        if (buff != null)
        {
          Array.Clear(buff, 0, buff.Length);
          buff = null;
        }
        Console.WriteLine("FTP Upload 완료");
      }
      catch (Exception ex)
      {
        //Logger._OutputErrorMsg(ex.Message);
        MessageBox.Show($"Upload 실패 \n{ ex.Message}");
        this.LastException = ex;
        System.Reflection.MemberInfo info = System.Reflection.MethodInfo.GetCurrentMethod();
        string id = string.Format("{0}.{1}", info.ReflectedType.Name, info.Name);
        if (this.ExceptionEvent != null)
          this.ExceptionEvent(id, ex);
        return false;
      }
      return true;
    }


    //thumbnail 파일 생성
    private string create_thumbnail(string _local_file_path)
    {
      if (File.Exists(_local_file_path.Insert(_local_file_path.LastIndexOf("/"), "/thumbnail")))
        return _local_file_path.Insert(_local_file_path.LastIndexOf("/"), "/thumbnail");
      try
      {
        //string file_name = _local_file_path.Substring(_local_file_path.LastIndexOf('\\'));
        string file_name = $"{_local_file_path.Substring(_local_file_path.LastIndexOf("/") + 1)}";

        if (File.Exists(_local_file_path))
        {
          string fileExtention = System.IO.Path.GetExtension(_local_file_path);
          fileExtention = fileExtention.ToUpper();
          using (FileStream fs = new FileStream(_local_file_path, FileMode.Open, FileAccess.Read))
          {
            Image img = Image.FromStream(fs, false, false);
            Image thumbnail = img.GetThumbnailImage(300, 300, delegate { return false; }, IntPtr.Zero);
            _local_file_path = _local_file_path.Insert(_local_file_path.LastIndexOf("/"), "/thumbnail");

            if (File.Exists(_local_file_path))
              File.Delete(_local_file_path);

            thumbnail.Save(_local_file_path);
            thumbnail.Dispose();

            img.Dispose();
            fs.Close();
          }
        }
      }
      catch (Exception _e)
      {
        //Logger._OutputErrorMsg(_e.Message);
      }

      return _local_file_path;
    }

    //  1) 서버에 파일 업로드 할 때 사용
    //서버에 폴더가 없다면 폴더를 만든다.
    private void make_directory(string _server_path)
    {
      if (!_server_path[_server_path.Length - 1].Equals('/'))
        _server_path = _server_path.Remove(_server_path.LastIndexOf("/") + 1);
      //if (check_server_directory_exist(_server_path))
      //return;


      string[] arr_directory = _server_path.Split('/');
      string currentDir = string.Empty;
      try
      {
        foreach (string tmpFoler in arr_directory)
        {
          try
          {
            if (tmpFoler == string.Empty)
              continue;
            currentDir += "/" + tmpFoler;
            string url = string.Format(@"FTP://{0}:{1}{2}", this.ftp_info.ip_addr, this.ftp_info.port, currentDir);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd);
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            ftpRequest.KeepAlive = false;
            ftpRequest.UsePassive = false;

            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();

            response.Close();
          }
          catch (Exception _e)
          {

            //MessageBox.Show(_e.Message);
          }
        }
      }
      catch (Exception ex)
      {

        this.LastException = ex;
        System.Reflection.MemberInfo info = System.Reflection.MethodInfo.GetCurrentMethod();
        string id = string.Format("{0}.{1}", info.ReflectedType.Name, info.Name);
        if (this.ExceptionEvent != null)
          this.ExceptionEvent(id, ex);
      }
    }
    private bool check_server_directory_exist(string _server_path)
    {


      return true;
    }

    //3. 서버에서 파일 다운로드
    private bool download(string _local_file_path, string _server_path)
    {
      try
      {
        check_directory(_local_file_path);

        string url = string.Format(@"FTP://{0}:{1}/{2}", this.ftp_info.ip_addr, this.ftp_info.port, _server_path);
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url); // "FTP://" 이기에 FtpWebRequest로 캐스팅 한 것
        ftpRequest.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd);
        ftpRequest.KeepAlive = false;
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = false;

        using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
        {
          using (FileStream outputStream = new FileStream(_local_file_path, FileMode.Create, FileAccess.Write))
          {
            using (Stream ftpStream = response.GetResponseStream())
            {
              int bufferSize = 2048;
              int readCount;
              byte[] buffer = new byte[bufferSize];

              readCount = ftpStream.Read(buffer, 0, bufferSize);
              while (readCount > 0)
              {
                outputStream.Write(buffer, 0, readCount);
                readCount = ftpStream.Read(buffer, 0, bufferSize);
              }
              ftpStream.Close();
              outputStream.Close();
              if (buffer != null)
              {
                Array.Clear(buffer, 0, buffer.Length);
                buffer = null;
              }
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        //Logger._OutputErrorMsg(ex.Message);
        MessageBox.Show($"Download Fail! \n{ex.Message}");
        this.LastException = ex;
        if (_server_path.Contains(@"\ZOOM\") == true)
          return false;
        System.Reflection.MemberInfo info = System.Reflection.MethodInfo.GetCurrentMethod();
        string id = string.Format("{0}.{1}", info.ReflectedType.Name, info.Name);
        if (this.ExceptionEvent != null)
          this.ExceptionEvent(id, ex);
        return false;
      }
    }


    //  1) 서버에서 파일 다운로드 할 때 사용
    //로컬에 폴더가 없다면 폴더를 만든다.
    private void check_directory(string _local_file_path)
    {
      FileInfo fInfo = new FileInfo(_local_file_path);
      if (!fInfo.Exists)
      {
        DirectoryInfo dInfo = new DirectoryInfo(fInfo.DirectoryName);
        if (!dInfo.Exists)
          dInfo.Create();
      }
    }

    //4. 해당 폴더에 담긴 파일 리스트 가져옴
    private List<string> get_file_list(string _server_path)
    {
      List<string> resultList = new List<string>();
      StringBuilder result = new StringBuilder();
      try
      {
        string url = string.Format(@"FTP://{0}:{1}/{2}", this.ftp_info.ip_addr, this.ftp_info.port, _server_path);
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
        ftpRequest.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd);
        ftpRequest.KeepAlive = false;
        ftpRequest.UseBinary = false;
        ftpRequest.UsePassive = false;
        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;

        using (WebResponse response = ftpRequest.GetResponse())
        {
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string line = reader.ReadLine();
          while (line != null)
          {
            result.Append(line);
            result.Append("\n");
            line = reader.ReadLine();
          }
          result.Remove(result.ToString().LastIndexOf('\n'), 1);
          if (reader != null)
            reader.Close();
          foreach (string file in result.ToString().Split('\n'))
            resultList.Add(file);
        }
        return resultList;
      }
      catch (Exception ex)
      {

        //MessageBox.Show(ex.Message);
        this.LastException = ex;
        System.Reflection.MemberInfo info = System.Reflection.MethodInfo.GetCurrentMethod();
        string id = string.Format("{0}.{1}", info.ReflectedType.Name, info.Name);
        if (this.ExceptionEvent != null)
          this.ExceptionEvent(id, ex);
        return resultList;
      }
    }

    //5. 파일 다이어로그 출력_여러 파일 선택 가능
    private List<string> open_dlg()
    {
      StringBuilder string_builder_filter = new StringBuilder(1024);
      string_builder_filter.Append("Image Files(*.jpg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png");
      string_builder_filter.Append("|BMP 파일(*.bmp)|*.bmp|Jpg 파일(*.jpg)|*.jpg|PNG 파일(*.png)|*.png");
      string_builder_filter.Append("|GIF 파일(*.gif)|*.gif");
      string_builder_filter.Append("|txt 파일(*.txt)|*.txt");

      System.Windows.Forms.OpenFileDialog open_file_dlg = new System.Windows.Forms.OpenFileDialog();
      //open_file_dlg.InitialDirectory = System.Environment.CurrentDirectory;         //초기경로
      open_file_dlg.RestoreDirectory = true;                 //현재 경로가 이전 경로로 복원되는지 여부          
      open_file_dlg.Filter = string_builder_filter.ToString();
      open_file_dlg.Title = "등록할 파일을 선택하세요.";
      open_file_dlg.Multiselect = true;                      //여러파일선택

      if (System.Windows.Forms.DialogResult.OK == open_file_dlg.ShowDialog())
      {
        string file_name = open_file_dlg.FileName; //Dialog의 결과이기에 이곳에서만 원본 이미지 파일의 경로를 참조할 수 있다.
        if (false == File.Exists(file_name))
          return null;//이미지 파일이 없으면 그냥 나감 ㅇㅇ
                      //return file_name;


        var arr_file_name = open_file_dlg.FileNames;


        return (from element in arr_file_name
                where true == is_img_file(element)
                select element).ToList<string>();
      }
      return null;
    }
    //220526_tk_image파일인지 확인
    private bool is_img_file(string _file_path)
    {
      string _file_extension = System.IO.Path.GetExtension(_file_path);
      if (_file_extension.Equals(".png") || _file_extension.Equals(".PNG")
      || _file_extension.Equals(".jpg") || _file_extension.Equals(".JPG")
      || _file_extension.Equals(".jpeg") || _file_extension.Equals(".JPEG")
      || _file_extension.Equals(".bmp") || _file_extension.Equals(".BMP")
      || _file_extension.Equals(".gif") || _file_extension.Equals(".GIF"))
        return true;
      System.Windows.Forms.MessageBox.Show("이미지 파일만 업로드할 수 있습니다.");
      return false;
    }

    //6. 파일 다이어로그 출력_저장 위치 선택
    private string get_save_file_path()
    {
      FolderBrowserDialog dialog = new FolderBrowserDialog();
      dialog.ShowDialog();
      string select_path = dialog.SelectedPath;    //선택한 다이얼로그 경로 저장

      return select_path;
    }

    //7. 서버의 특정 폴더에 저장된 파일 삭제 
    private bool delete_saved_file_in_server(string _server_file_path)
    {
      bool is_delete = false;
      try
      {
        string url = string.Format(@"FTP://{0}:{1}/{2}", this.ftp_info.ip_addr, this.ftp_info.port, _server_file_path);
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
        ftpRequest.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd);
        ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

        using (WebResponse response = ftpRequest.GetResponse())
        {

        }
        is_delete = true;
        ftpRequest = null;
      }
      catch (Exception _e)
      {
        //Logger._OutputErrorMsg(_e.Message);
        MessageBox.Show(_e.Message);
      }
      return is_delete;
    }
    //7_2. 서버에서 여러 파일 다운로드
    private bool delete_saved_file_in_server(List<string> _list_server_file_path)
    {
      Action<string> action = (string __server_path) => delete_saved_file_in_server(__server_path);
      var list_thread = new List<Thread>();
      _list_server_file_path.ForEach(element => list_thread.Add(new Thread(() => action(element))));
      list_thread.ForEach(element => { element.Start(); element.Join(); });

      return true;
    }
    //7_3. 서버의 특정 폴더에 저장된 파일 전체 삭제 
    private bool delete_all_saved_file_in_server(string _server_path)
    {
      Action<string> action = (string __server_path) => delete_saved_file_in_server(__server_path);
      var list_server_file_path = (from element in GetFileList(_server_path) select element.Insert(0, _server_path)).ToList();
      var list_thread = new List<Thread>();
      list_server_file_path.ForEach(element => list_thread.Add(new Thread(() => action(element))));
      list_thread.ForEach(element => element.Start());
      return true;
    }



    #endregion


    #region 다중파일 업로드, 다운로드 _명월일지
    // FTP 서버 접속 함수
    private FtpWebResponse Connect(String url, string method, Action<FtpWebRequest> action = null)
    {
      // WebRequest 클래스를 이용해 접속하기 때문에 객체를 가져온다. (FtpWebRequest로 변환)
      var request = WebRequest.Create(url) as FtpWebRequest;
      // Binary 형식으로 사용한다.
      request.UseBinary = true;
      // FTP 메소드 설정(아래에 별도 설명)
      request.Method = method;
      // 로그인 인증
      request.Credentials = new NetworkCredential(ftp_info.user, ftp_info.pwd);
      // request.GetResponse()함수가 호출되면 실제적으로 접속이 되기 때문에, 그전에 설정할 callback 함수 호출
      if (action != null)
      {
        action(request);
      }
      // 접속해서 WebResponse함수를 가져온다.
      return request.GetResponse() as FtpWebResponse;
    }


    // 업로드 함수
    private void UploadFileList(String url, string source)
    {
      // 업로드할 경로의 속성을 구한다.
      var attr = File.GetAttributes(source);
      // 만약 디렉토리라면..
      if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
      {
        // 디렉토리 정보를 가져온다.
        DirectoryInfo dir = new DirectoryInfo(source);
        // 디렉토리 안의 파일 리스트를 가져온다.
        foreach (var item in dir.GetFiles())
        {
          // 파일을 업로드한다.
          UploadFileList(url + "/" + item.Name, item.FullName);
        }
        // 디렉토리 안의 하위 디렉토리 리스트를 가져온다.
        foreach (var item in dir.GetDirectories())
        {
          try
          {
            // ftp에 디렉토리를 생성한다.
            Connect(url + "/" + item.Name, WebRequestMethods.Ftp.MakeDirectory).Close();
          }
          catch (WebException)
          {
            // 만약에 ftp에 디렉토리가 존재한다면 에러가 날 것이다.
          }
          // 디렉토리를 업로드한다.(재귀 함수 호출)
          UploadFileList(url + "/" + item.Name, item.FullName);
        }
      }
      else
      {
        // 디렉토리가 아닌 파일을 경우인데, 파일의 stream을 취득한다.
        using (var fs = File.OpenRead(source))
        {
          // 파일을 업로드한다.
          Connect(url, WebRequestMethods.Ftp.UploadFile, (req) =>
          {
            // 파일 크기 설정
            req.ContentLength = fs.Length;
            // GetResponse()가 호출되기 전에 request스트림에 파일 binary를 넣는다.
            using (var stream = req.GetRequestStream())
            {
              fs.CopyTo(stream);
            }
          }).Close();
          // respose 객체를 닫는다.
        }
      }
    }


    // 다운로드 함수
    private void DownloadFileList(string url, string target)
    {
      var list = new List<String>();
      // ftp에 접속해서 파일과 디렉토리 리스트를 가져온다.
      using (var res = Connect(url, WebRequestMethods.Ftp.ListDirectory))
      {
        using (var stream = res.GetResponseStream())
        {
          using (var rd = new StreamReader(stream))
          {
            while (true)
            {
              // binary 결과에서 개행(\r\n)의 구분으로 파일 리스트를 가져온다.
              string buf = rd.ReadLine();
              // null이라면 리스트 검색이 끝난 것이다.
              if (string.IsNullOrWhiteSpace(buf))
              {
                break;
              }
              list.Add(buf);
            }
          }
        }
      }
      // ftp 리스트를 돌린다.
      foreach (var item in list)
      {
        try
        {
          // 파일을 다운로드한다.
          using (var res = Connect(url + "/" + item, WebRequestMethods.Ftp.DownloadFile))
          {
            using (var stream = res.GetResponseStream())
            {
              // stream을 통해 파일을 작성한다.
              using (var fs = File.Create(target + "\\" + item))
              {
                stream.CopyTo(fs);
              }
            }
          }
        }
        catch (WebException)
        {
          // 그러나 파일이 아닌 디렉토리의 경우는 에러가 발생한다.
          // 로컬 디렉토리를 만든다.
          Directory.CreateDirectory(target + "\\" + item);
          // 디렉토리라면 재귀적 방법으로 다시 파일리스트를 탐색한다.
          DownloadFileList(url + "/" + item, target + "\\" + item);
        }
      }
    }

    public void Dispose()
    {
      
    }




    #endregion



  }
}
