using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine
{
    public enum PAGE_CMD : int
    {
        cur_page_num
        , view_board_cnt
        , view_page_cnt
        , total_board_cnt
        , total_page_cnt
        , cur_page_group_num
        , cur_start_page_num
        , cur_end_page_num
        , cur_start_board_num
        , cur_end_board_num
    };

    public class CPaging
    {
        public delegate object del_func(params object[] _arr_obj_sender);
        public CPaging()
        {

        }

        private const int DEFAULT_BUF_CAP = 1024;
        private const int DEFAULT_VIEW_BOARD_CNT = 15;
        private const int DEFAULT_VIEW_PAGE_CNT = 1;

        //public int view_board_cnt = 0;
        //public int view_page_cnt = 0;

        public Dictionary<PAGE_CMD, int> create_page_info(int _cur_page_num, int _total_board_cnt)
        {
            // 1) 현재 페이지 번호
            // 2) DEFUALT_한 번에 볼 수 있는 게시글의 개수 
            // 3) DEFUALT_한 번에 볼 수 있는 페이지의 개수
            // 4) 전체 게시글의 개수
            // 5) 전체 페이지의 개수
            // 6) 현재 페이지의 그룹번호 
            // 7) 현재 페이지 그룹의 시작 페이지 번호 
            // 8) 현재 페이지 그룹의 마지막 페이지 번호 
            Dictionary<PAGE_CMD, int> map_page_info = new Dictionary<PAGE_CMD, int>();
            int cur_page_num = 0;
            int view_board_cnt = 0;
            int view_page_cnt = 0;
            int total_board_cnt = 0;
            int total_page_cnt = 0;
            int cur_page_group_num = 0;
            int cur_start_page_num = 0;
            int cur_end_page_num = 0;
            int cur_start_board_num = 0;
            int cur_end_board_num = 0;

            // 1) 현재 페이지 번호
            cur_page_num = _cur_page_num;
            // 2) DEFUALT_한 번에 볼 수 있는 게시글의 개수 
            view_board_cnt = DEFAULT_VIEW_BOARD_CNT;
            // 3) DEFUALT_한 번에 볼 수 있는 페이지의 개수
            view_page_cnt = DEFAULT_VIEW_PAGE_CNT;
            // 4) 전체 게시글의 개수
            total_board_cnt = _total_board_cnt;
            // 5) 전체 페이지의 개수
            total_page_cnt = ((total_board_cnt % view_board_cnt) == 0) ? (total_board_cnt / view_board_cnt) : (total_board_cnt / view_board_cnt) + 1;
            // 6) 현재 페이지의 그룹번호 
            cur_page_group_num = ((cur_page_num % view_page_cnt) == 0) ? (cur_page_num / view_page_cnt) : (cur_page_num / view_page_cnt) + 1;
            // 7) 현재 페이지 그룹의 시작 페이지 번호 
            cur_start_page_num = (((cur_page_group_num - 1) * view_page_cnt) + 1);
            // 8) 현재 페이지 그룹의 마지막 페이지 번호            
            cur_end_page_num = ((cur_page_group_num * view_page_cnt) <= total_page_cnt) ? (cur_page_group_num * view_page_cnt) : total_page_cnt;
            // 9) 현재 페이지의 시작 게시글 번호
            cur_start_board_num = ((cur_page_num - 1) * view_board_cnt) + 1;
            // 10) 현재 페이지의 마지막 게시글 번호 
            cur_end_board_num = ((cur_page_group_num * view_page_cnt) <= total_page_cnt)
                ? cur_start_board_num + view_board_cnt - 1
                : total_board_cnt;

            map_page_info.Add(PAGE_CMD.cur_page_num, cur_page_num);
            map_page_info.Add(PAGE_CMD.view_board_cnt, view_board_cnt);
            map_page_info.Add(PAGE_CMD.view_page_cnt, view_page_cnt);
            map_page_info.Add(PAGE_CMD.total_board_cnt, total_board_cnt);
            map_page_info.Add(PAGE_CMD.total_page_cnt, total_page_cnt);
            map_page_info.Add(PAGE_CMD.cur_page_group_num, cur_page_group_num);
            map_page_info.Add(PAGE_CMD.cur_start_page_num, cur_start_page_num);
            map_page_info.Add(PAGE_CMD.cur_end_page_num, cur_end_page_num);
            map_page_info.Add(PAGE_CMD.cur_start_board_num, cur_start_board_num);
            map_page_info.Add(PAGE_CMD.cur_end_board_num, cur_end_board_num);

            return map_page_info;
        }


    }
}
