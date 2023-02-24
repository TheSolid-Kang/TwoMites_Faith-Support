using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TwoMites_Engine._99.Headers
{
    public class CDelegateCommand : ICommand
    {
        private readonly Predicate<object> m_can_execute;
        private readonly Action<object>? m_execute;
        public event EventHandler? CanExecuteChanged;

        public CDelegateCommand(Action<object> execute)
            : this(execute, (object _obj) => true)
        { }
        public CDelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            m_execute = execute;
            m_can_execute = canExecute;
        }

        public bool CanExecute(object _obj) => m_can_execute.Invoke(_obj);

        public void Execute(object _obj) => m_execute?.Invoke(_obj);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    }
}
