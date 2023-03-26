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

    }
}
