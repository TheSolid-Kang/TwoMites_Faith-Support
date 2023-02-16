using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoMites_Engine._02.DTO._03.Fellowship;

namespace TwoMites_Engine._01.DAO._03.CFellowship_DAO
{
  internal class CFellowship_DAO : DAO_MySQL_v2
  {
    public CFellowship_DAO() { }
    ~CFellowship_DAO() { Dispose(); }

    private DAO_MySQL_v2 dao;
    private const int DEFAULT_CAP = 1024;

    #region 멤버함수 선언부
    /*FELLOWSHIP_DEPARTMENT 목록 가져오는 함수
     */
    public ObservableCollection<FELLOWSHIP_DEPARTMENT> SelectListFellowshipDepartment() => select_list_fellowship_department();

    /*Fellowship 테이블
     * 새등록(새로운 교제 등록)
     */
    public object InsertFellowship(int f_fd_id, DateTime f_created_at, string f_descript) 
      => insert_fellowship( f_fd_id, f_created_at, f_descript);
    #endregion


    #region 멤버함수 정의부
    /*FELLOWSHIP_DEPARTMENT 목록 가져오는 함수
     */
    private ObservableCollection<FELLOWSHIP_DEPARTMENT> select_list_fellowship_department()
    {
      StringBuilder query = new StringBuilder(DEFAULT_CAP);
      query.Append("SELECT * FROM TwoMites.FELLOWSHIP_DEPARTMENT");
      var list_fellowship_department = new ObservableCollection<FELLOWSHIP_DEPARTMENT>();
      using (var dao = new DAO_MySQL_v2())
      {
        using (var data_table = dao.GetDataTable(query.ToString()))
        {
          for (int i = 0; i < data_table.Rows.Count; i++)
          {
            list_fellowship_department.Add(new FELLOWSHIP_DEPARTMENT()
            {
              fd_pk_id = int.Parse(data_table.Rows[i]["fd_pk_id"].ToString())
              , fd_department = data_table.Rows[i]["fd_department"].ToString()
            });
          }
          
          return list_fellowship_department;
        }
      }
      return null;
    }

    /*Fellowship 테이블
     * 새등록(새로운 교제 등록)
     */
    private object insert_fellowship(int f_fd_id, DateTime f_created_at, string f_descript) 
    {
      using(dao = new DAO_MySQL_v2())
      {
        StringBuilder str_buil = new StringBuilder(DEFAULT_CAP);
        str_buil.Append("INSERT INTO TwoMites.FELLOWSHIP(f_fd_id, f_created_at, f_descript) VALUESE ");
        str_buil.Append($"({f_fd_id}, {f_created_at}, '{f_descript}');");
        using (var data_table = dao.GetDataTable(str_buil.ToString()))
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
