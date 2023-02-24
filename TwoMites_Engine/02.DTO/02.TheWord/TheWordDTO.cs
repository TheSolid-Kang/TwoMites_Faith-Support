using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._02.DTO._02.TheWord
{
    public class TheWordDTO
    {
        public TheWordDTO() { }
        public TheWordDTO(TheWordDTO? _the_word_dto)
        {
            this.tw_pk_id = _the_word_dto.tw_pk_id;
            this.tw_wt_key = _the_word_dto.tw_wt_key;
            this.tw_pastor = _the_word_dto.tw_pastor;
            this.tw_created_at = _the_word_dto.tw_created_at;
            this.tw_modified_at = _the_word_dto.tw_modified_at;
            this.tw_title = _the_word_dto.tw_title;
            this.tw_the_word = _the_word_dto.tw_the_word;
        }


        public TheWordDTO(int tw_pk_id, int tw_wt_key, string? tw_pastor, DateTime tw_date, DateTime? tw_created_at, DateTime? tw_modified_at, string? tw_title, string? tw_the_word)
        {
            this.tw_pk_id = tw_pk_id;
            this.tw_wt_key = tw_wt_key;
            this.tw_pastor = tw_pastor;
            this.tw_date = tw_date;
            this.tw_created_at = tw_created_at;
            this.tw_modified_at = tw_modified_at;
            this.tw_title = tw_title;
            this.tw_the_word = tw_the_word;
        }

        public int tw_pk_id { get; set; } = 0;
        public int tw_wt_key { get; set; } = 0;
        public string? tw_pastor { get; set; } = string.Empty;
        public DateTime tw_date { get; set; } = DateTime.Today;
        public DateTime? tw_created_at { get; set; }
        public DateTime? tw_modified_at { get; set; }
        public string? tw_title { get; set; } = string.Empty;
        public string? tw_the_word { get; set; } = string.Empty;



    }
}
