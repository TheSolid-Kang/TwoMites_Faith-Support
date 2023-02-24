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
using TwoMites._02.CCommon;
using TwoMites_Engine._02.DTO._01.BibleDTO;

namespace TwoMites._01.Page._00.Bible
{
    /// <summary>
    /// BibleMainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BibleMainView : UserControl, IViewBase
    {
        public BibleMainView()
        {
            InitializeComponent();
            _viewModel = new BibleMainViewModel();
            this.DataContext = _viewModel;
            _sideCommander.DataContext = _viewModel;
            CCommon_SearchView.SetDataContext(_viewModel);
        }
        private BibleMainViewModel _viewModel { get; set; }
        private CSideCommander _sideCommander = new CSideCommander();

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {
        }

        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {
            _viewModel?.Dispose();
            _sideCommander?.Dispose();
            _viewModel = null;
            _sideCommander = null;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private void DG_ListSummary_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                e.Handled = true;
                var dto = DG_ListSummary.SelectedItem as BibleSummaryDto;
                if (true == dto.bs_book.Equals(""))
                {
                    _viewModel.InsertBibleSummary.Execute(null);
                }
            }
        }

        private void DG_ListContemplation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                e.Handled = true;
                _viewModel.InsertBibleContemplation.Execute(null);
            }
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnUpdateSummary_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnDeleteSummary_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var temp = DG_ListSummary.SelectedItem;
        }
    }
}