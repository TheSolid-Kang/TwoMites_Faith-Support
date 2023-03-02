using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            CCommon_SearchView.DataContext = _viewModel;
            CCommon_SideCommander.DataContext = _viewModel;
        }
        private BibleMainViewModel _viewModel { get; set; }

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

        private void DG_ListSummary_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = e.OriginalSource as System.Windows.Controls.TextBox;
            var content = textBox?.Text.ToString();
            _viewModel.TB_BibleSummary = content;
        }

        private void DG_ListContemplation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = e.OriginalSource as System.Windows.Controls.TextBox;
            var content = textBox?.Text.ToString();
            _viewModel.TB_BibleContemplation = content;
        }

        private void TB_AddSummary(object sender, MouseButtonEventArgs e)
        {
            var obsSummaryDto = DG_ListSummary.ItemsSource as ObservableCollection<BibleSummaryDto>;
            var focusBible = _viewModel.LV_FocusBibleItem;
            obsSummaryDto.Add(new BibleSummaryDto(focusBible.b_pk_id, focusBible.b_book, focusBible.b_chapter, focusBible.b_verse));
            DG_ListSummary.ItemsSource = obsSummaryDto;
        }

        private void TB_UpdateSummary_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var summaryDto = DG_ListSummary.SelectedItem as BibleSummaryDto;
            if (null == summaryDto)
            {
                return;
            }

            if (0 == summaryDto.bs_pk_id)
            {
                _viewModel.InsertBibleSummary.Execute(null);
            }
            else
            {
                _viewModel.TB_BibleSummary = summaryDto.bs_descript;
                _viewModel.UpdateBibleSummary.Execute(summaryDto?.bs_pk_id);
            }
        }

        private void TB_DeleteSummary_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var summaryDto = DG_ListSummary.SelectedItem as BibleSummaryDto;
            if (null == summaryDto)
            {
                return;
            }

            if (0 == summaryDto.bs_pk_id)
            {
                var obsSummaryDto = DG_ListSummary.ItemsSource as ObservableCollection<BibleSummaryDto>;
                obsSummaryDto.Remove(summaryDto);
                DG_ListSummary.ItemsSource = obsSummaryDto;
            }
            else
            {
                _viewModel.DeleteBibleSummary.Execute(summaryDto?.bs_pk_id);
            }
        }



        private void TB_AddContemplation(object sender, MouseButtonEventArgs e)
        {
            var obsContemplationDto = DG_ListContemplation.ItemsSource as ObservableCollection<BibleContemplationDto>;
            var focusBible = _viewModel.LV_FocusBibleItem;
            obsContemplationDto.Add(new BibleContemplationDto(focusBible.b_pk_id, focusBible.b_book, focusBible.b_chapter, focusBible.b_verse));
            DG_ListContemplation.ItemsSource = obsContemplationDto;
        }

        private void TB_UpdateContemplation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var contemplationDto = DG_ListContemplation.SelectedItem as BibleContemplationDto;
            if (null == contemplationDto)
            {
                return;
            }

            if (0 == contemplationDto.bc_pk_id)
            {
                _viewModel.InsertBibleContemplation.Execute(null);
            }
            else
            {
                _viewModel.TB_BibleContemplation = contemplationDto.bc_descript;
                _viewModel.UpdateBibleContemplation.Execute(contemplationDto?.bc_pk_id);
            }
        }

        private void TB_DeleteContemplation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var contemplationDto = DG_ListContemplation.SelectedItem as BibleContemplationDto;
            if(null == contemplationDto)
            {
                return;
            }

            if (0 == contemplationDto.bc_pk_id)
            {
                var obsContemplationDto = DG_ListContemplation.ItemsSource as ObservableCollection<BibleContemplationDto>;
                obsContemplationDto.Remove(contemplationDto);
                DG_ListContemplation.ItemsSource = obsContemplationDto;
            }
            else
            {
                _viewModel.DeleteBibleContemplation.Execute(contemplationDto?.bc_pk_id);
            }
        }
    }
}