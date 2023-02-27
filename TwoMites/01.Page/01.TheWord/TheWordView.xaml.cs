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
            m_view_model = new TheWordViewModel();
            this.DataContext = m_view_model;
            m_side_commander.DataContext = m_view_model;
        }
        private TheWordViewModel? m_view_model { get; set; }
        private CSideCommander? m_side_commander { get; set; } = new CSideCommander();

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {
        }

        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {
            m_view_model?.Dispose();
            m_side_commander?.Dispose();
            m_view_model = null;
            m_side_commander = null;
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
