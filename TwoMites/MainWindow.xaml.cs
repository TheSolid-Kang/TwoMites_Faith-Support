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
using TwoMites._01.Page._00.Bible;
using TwoMites._01.Page._01.TheWord;
using TwoMites._01.Page._03.Fellowship;
using TwoMites._02.CCommon;

namespace TwoMites
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewBase
    {
        public MainWindow()
        {
            InitializeComponent();

            m_view_model = new MainWindowViewModel();
            this.DataContext = m_view_model;
        }
        MainWindowViewModel m_view_model;

        ~MainWindow()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public void Load(object _obj, RoutedEventArgs _routed_event_args)
        {
            main_grid.Children.Clear();

            main_grid.Children.Add(new BibleMainView());

        }
        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
        {

        }

        private void ClickBiblePage(object sender, RoutedEventArgs e)
        {
            main_grid.Children.Clear();
            main_grid.Children.Add(new BibleMainView());
        }
        private void ClickWordPage(object sender, RoutedEventArgs e)
        {
            main_grid.Children.Clear();
            main_grid.Children.Add(new TheWordView());
        }

        private void ClickPrayPage(object sender, RoutedEventArgs e)
        {
            main_grid.Children.Clear();
            main_grid.Children.Add(new TheWordView());
        }
        private void ClickFellowshipPage(object sender, RoutedEventArgs e)
        {
            main_grid.Children.Clear();
            main_grid.Children.Add(new FellowshipMainView());
        }

        private void ClickLogo(object sender, RoutedEventArgs e)
        {
            main_grid.Children.Clear();
            main_grid.Children.Add(new BibleMainView());
        }
    }
}
