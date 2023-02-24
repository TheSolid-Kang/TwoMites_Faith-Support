using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwoMites._01.Page
{
    internal interface IViewBase : IDisposable
    {
        public void Load(object _obj, RoutedEventArgs _routed_event_args);
        public void UnLoad(object _obj, RoutedEventArgs _routed_event_args);

    }
}
