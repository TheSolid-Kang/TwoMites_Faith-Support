using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._02.DTO._03.Fellowship;

namespace TwoMites_Engine._01.DAO._03.CFellowship_DAO
{
    internal class CFellowship_DAO : Engine._01.DAO.MySQL_DAO_v3
    {
        public CFellowship_DAO() { }
        ~CFellowship_DAO() { Dispose(); }

        private Engine._01.DAO.MySQL_DAO_v3 dao;
        private const int DEFAULT_CAP = 1024;

        #region 멤버함수 선언부
        /*FellowshipDepartmentDto 목록 가져오는 함수
         */
        public ObservableCollection<FellowshipDepartmentDto> SelectListFellowshipDepartment() => _SelectListFellowshipDepartment();

        /*Fellowship 테이블
         * 새등록(새로운 교제 등록)
         */
        public object InsertFellowship(int f_fd_id, DateTime f_created_at, string f_descript) => _InsertFellowship(f_fd_id, f_created_at, f_descript);
        #endregion


        #region 멤버함수 정의부
        /*FellowshipDepartmentDto 목록 가져오는 함수
         */
        private ObservableCollection<FellowshipDepartmentDto> _SelectListFellowshipDepartment()
        {
            StringBuilder query = new StringBuilder(DEFAULT_CAP);
            query.Append("SELECT * FROM TwoMites.FellowshipDepartmentDto");
            var list_FellowshipDepartmentDto = new ObservableCollection<FellowshipDepartmentDto>();
            using (var dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                using (var dataTable = dao.GetDataTable(query.ToString()))
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        list_FellowshipDepartmentDto.Add(new FellowshipDepartmentDto()
                        {
                            fd_pk_id = int.Parse(dataTable.Rows[i]["fd_pk_id"].ToString())
                          ,
                            fd_department = dataTable.Rows[i]["fd_department"].ToString()
                        });
                    }

                    return list_FellowshipDepartmentDto;
                }
            }
            return null;
        }

        /*Fellowship 테이블
         * 새등록(새로운 교제 등록)
         */
        private object _InsertFellowship(int f_fd_id, DateTime f_created_at, string f_descript)
        {
            using (dao = new Engine._01.DAO.MySQL_DAO_v3())
            {
                StringBuilder str_buil = new StringBuilder(DEFAULT_CAP);
                str_buil.Append("INSERT INTO TwoMites.FELLOWSHIP(f_fd_id, f_created_at, f_descript) VALUESE ");
                str_buil.Append($"({f_fd_id}, {f_created_at}, '{f_descript}');");
                using (var dataTable = dao.GetDataTable(str_buil.ToString()))
                {

                }

                str_buil.Clear();
            }
            return null;
        }


        public void Dispose()
        {
            dao?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
