using System;
using System.Collections.Generic;
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
using TwoMites._01.Page;

namespace TwoMites._02.CCommon
{
    /// <summary>
    /// SearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchView : UserControl, IViewBase
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private DateTime _FromDate = DateTime.Today;
        private DateTime _ToDate = DateTime.Today;

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {
            FromDate.SelectedDate = _FromDate;
            ToDate.SelectedDate = _ToDate;
        }

        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {

        }

        public void SetDataContext(ViewModelBase _viewModelBase) 
        {
            DataContext = _viewModelBase;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}