using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwoMites._02.CCommon;

namespace TwoMites._01.Page._01.TheWord
{
    /// <summary>
    /// TheWordView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TheWordView : UserControl, IViewBase
    {
        public TheWordView()
        {
            InitializeComponent();
            _viewModel = new TheWordViewModel();
            this.DataContext = _viewModel;
            CCommon_SearchView.DataContext = _viewModel;
            CCommon_SideCommander.DataContext = _viewModel;
        }
        private TheWordViewModel? _viewModel { get; set; }

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {
        }

        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {
            _viewModel?.Dispose();
            _viewModel = null;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }



        private void Grid_Drop(object sender, DragEventArgs e)
        {
            List<string> listFilePath = GetDropFilesPaths(e);

            L_FilePath.Content = "";
            foreach (string _filePath in listFilePath)
            {
                L_FilePath.Content += _filePath + "\n";
            }
        }

        /// <summary>
        /// GetDropFilesPaths 드랍파일 가져오기
        /// </summary>
        /// <param name="e">DropData</param>
        /// <returns>DropFiles</returns>
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

    

        private void LB_FileDrop_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                // 드래그된 소스 ListViewItem
                var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;

                // ListViewItem를 Clone하여 추가
                LB_FileDrop.Items.Add(item as ListViewItem);
            }

        }

        private void LB_FileDrop_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

    }
}
