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
using TwoMites._01.Page;

namespace TwoMites._02.CCommon
{
  /// <summary>
  /// CSideCommander.xaml에 대한 상호 작용 논리
  /// </summary>
  public partial class CSideCommander : UserControl, IViewBase
  {
    public CSideCommander()
    {
      InitializeComponent();
    }
    ~CSideCommander()
    {
      Dispose();
    }

    public void Load(object _obj, RoutedEventArgs _routed_event_args)
    {

    }
    public void UnLoad(object _obj, RoutedEventArgs _routed_event_args)
    {

    }

    public void Dispose()
    {
      GC.SuppressFinalize(true);
    }


  }
}
