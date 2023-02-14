﻿using System;
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
      m_view_model = new BibleMainViewModel();
      this.DataContext = m_view_model;
      m_side_commander.DataContext = m_view_model;
    }
    private BibleMainViewModel m_view_model { get; set; }
    private CSideCommander m_side_commander = new CSideCommander();

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
  }
}