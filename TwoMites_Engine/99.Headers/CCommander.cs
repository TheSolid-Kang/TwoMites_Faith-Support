using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace TwoMites_Engine._99.Headers
{
    public class CCommander
    {
        public CCommander() : this("EMPTY_Commander", new CDelegateCommand((object _obj) => { MessageBox.Show("this is empty commander"); }), "") { }
        public CCommander(string name, ICommand command) : this(name, command, "") { }
        public CCommander(string name, ICommand command, string buttonIconPath)
        {
            _name = name;
            _img_path = buttonIconPath;
            _cmd = command;
        }
        public CCommander(string name, ICommand command, string buttonIconPath, bool _is_enable) : this(name, command, buttonIconPath)
        => this._is_enable = _is_enable;


        protected string _name;
        protected string _img_path;
        protected ICommand _cmd;
        private bool _is_enable = true;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ImgPath
        {
            get { return _img_path; }
            set { _img_path = value; }
        }

        public ICommand Command
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        public bool Enable => _is_enable;
    }
}
