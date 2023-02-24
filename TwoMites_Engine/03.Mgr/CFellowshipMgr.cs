using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._01.DAO._03.CFellowship_DAO;
using TwoMites_Engine._02.DTO._03.Fellowship;

namespace TwoMites_Engine._03.Mgr
{
    public class CFellowshipMgr : GENERIC_MGR<CFellowshipMgr>
    {
        public CFellowshipMgr()
        {

        }
        ~CFellowshipMgr()
        {

        }

        private CFellowship_DAO dao = new CFellowship_DAO();

        public bool CreateFellowship()
        {
            return true;
        }
        public ObservableCollection<FellowshipDepartmentDto>? SelectListFellowshipDepartment() => dao.SelectListFellowshipDepartment();
    }
}
