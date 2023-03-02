using Engine._01.DAO;
using Engine._08.CFileMgr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwoMites._01.Page;

namespace TwoMites._02.CCommon.Popup
{
    /// <summary>
    /// FileSelectView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileSelectView : Window, IViewBase
    {
        public FileSelectView()
        {
            InitializeComponent();
        }        
        public FileSelectView(ViewModelBase _viewModelBase) : this()
        {
            DataContext = _viewModelBase;
        }
        ~FileSelectView() 
        {
            LB_Contents.Items?.Clear();
            DG_FileNames.Items?.Clear();
        }
        

        public void Dispose()
        {

        }

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {

        }

        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {

        }

        public void SetDataContext(ViewModelBase _viewModelBase)
        {
            DataContext = _viewModelBase;
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {

        }

        private void G_FileNames_Drop(object sender, DragEventArgs e)
        {
            DG_FileNames.Items?.Clear();
            List<string> listFilePath = GetDropFilesPaths(e);
            AddFileNames(listFilePath);
        }
        private void TB_AddFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DG_FileNames.Items?.Clear();
            List<string> listFilePath = CFileMgr.GetInstance().GetFileNames();
            AddFileNames(listFilePath);
        }
        private void AddFileNames(List<string> _listFileName)
        {
            _listFileName.Sort();
            _listFileName.Reverse();
            foreach (string _filePath in _listFileName)
            {
                FileInfo fileInfo = new FileInfo(_filePath);
                var extension = fileInfo.Extension.ToLower();
                if (extension == ".txt" || extension == ".md")
                    DG_FileNames.Items?.Add(fileInfo);
            }
        }
        private List<string> GetDropFilesPaths(DragEventArgs e, bool deduplication = false)
        {
            string[] arrDataFile = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> listFile = new List<string>();

            foreach (string DataFile in arrDataFile)
            {
                try
                {
                    // 폴더이면 하위폴더 파일가져오기
                    if ((File.GetAttributes(DataFile) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        var files = Directory.EnumerateFiles(DataFile, "*.*", SearchOption.AllDirectories);   // 하위폴더 포함
                        listFile.AddRange(files);
                    }
                    // 파일이면 파일가져오기
                    else if ((File.GetAttributes(DataFile) & FileAttributes.Directory) != FileAttributes.Directory)
                    {
                        listFile.Add(DataFile);
                    }
                }
                catch
                {
                    MessageBox.Show("가져오지 못한 파일 또는 폴더가 있습니다.");
                    continue;
                }
            }

            // 중복데이터제거
            if (deduplication)
                listFile.Distinct();

            return listFile;
        }

        private void TB_DeleteFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DG_FileNames.Items?.Remove(DG_FileNames.SelectedItem);
        }
        private void DG_FileNames_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            LB_Contents.Items.Clear();
            foreach(var _file in dataGrid.SelectedItems)
            {
                var fileInfo = _file as FileInfo;

                LB_Contents.Items.Add(fileInfo.FullName);
                foreach (var _line in CFileMgr.GetInstance().GetFileTextIter(fileInfo.FullName))
                {
                    LB_Contents.Items.Add(_line);
                }
                LB_Contents.Items.Add("");
                LB_Contents.Items.Add("");
            }

        }
    }
}
