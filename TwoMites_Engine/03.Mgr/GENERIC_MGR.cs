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
    public DAO_MySQL m_dao_mysql => new DAO_MySQL();
    public DAO_MySQL_v2 m_dao_mysql_v2 => new DAO_MySQL_v2();
    public DAO_MySQL_v2 Get_MySQL_V2(string _name) => new DAO_MySQL_v2(_name);
    //DAO를 호출 할 때마다 새로운 DAO 객체를 생성한다.
    //DAO 자체에 Disable 만들어야한다.
  }
}