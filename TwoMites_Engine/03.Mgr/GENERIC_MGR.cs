using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._01.DAO;

namespace TwoMites_Engine._03.Mgr
{
    public class GENERIC_MGR<T> where T : GENERIC_MGR<T>, new()
    {
        private static readonly Lazy<T> _pInstance = new Lazy<T>(() => new T());//쓰레드 환경에서 안전한 Lazy 
        public static T m_pInstance => _pInstance.Value;
    }
}