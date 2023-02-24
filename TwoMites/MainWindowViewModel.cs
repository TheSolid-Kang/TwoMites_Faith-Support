using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites._01.Page;

namespace TwoMites
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            m_engineTwoMites?.m_bible_mgr.ToString();

        }
        ~MainWindowViewModel()
        {

        }



    }
}
