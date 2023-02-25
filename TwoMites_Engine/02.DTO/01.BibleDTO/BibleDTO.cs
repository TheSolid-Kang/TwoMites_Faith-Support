using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._02.DTO._01.BibleDTO
{
    public enum EDTO_BIBLE : int
    {
        b_pk_id, b_book, b_chapter
    , b_verse, b_descript, b_full_descript
    }
    public class BibleDto
    {
        public BibleDto()
          : this(0, "", "", "", "", "")
        { }
        public BibleDto(string b_book, string b_chapter, string b_verse)
          : this(0, b_book, b_chapter, b_verse, "", "")
        {
            this.b_book = b_book;
            this.b_chapter = b_chapter;
            this.b_verse = b_verse;
        }
        public BibleDto(string b_book, string b_chapter, string b_verse, string b_descript, string b_full_descript)
            : this(0, b_book, b_chapter, b_verse, b_descript, b_full_descript)
        {
        }
        public BibleDto(int b_pk_id, string b_book, string b_chapter, string b_verse, string b_descript, string b_full_descript)
        {
            this.b_pk_id = b_pk_id;
            this.b_book = b_book;
            this.b_chapter = b_chapter;
            this.b_verse = b_verse;
            this.b_descript = b_descript;
            this.b_full_descript = b_full_descript;
        }
        public int b_pk_id { get; set; }
        public string? b_book { get; set; }
        public string? b_chapter { get; set; }
        public string? b_verse { get; set; }
        public string? b_descript { get; set; }
        public string? b_full_descript { get; set; }
    }

    public class BibleSummaryDto
    {
        public BibleSummaryDto(int b_fk_id, string? bs_book, string? bs_chapter, string? bs_verse)
        {
            this.b_fk_id = b_fk_id;
            this.bs_book = bs_book;
            this.bs_chapter = bs_chapter;
            this.bs_verse = bs_verse;
        }

        public BibleSummaryDto(int bs_pk_id, int b_fk_id, DateTime? bs_date, string? bs_book
            , string? bs_chapter, string? bs_verse, string? bs_descript)
        {
            this.bs_pk_id = bs_pk_id;
            this.b_fk_id = b_fk_id;
            this.bs_date = bs_date;
            this.bs_book = bs_book;
            this.bs_chapter = bs_chapter;
            this.bs_verse = bs_verse;
            this.bs_descript = bs_descript;
        }

        public int bs_pk_id { get; set; }
        public int b_fk_id { get; set; }
        public DateTime? bs_date { get; set; }
        public string? bs_book { get; set; }
        public string? bs_chapter { get; set; }
        public string? bs_verse { get; set; }
        public string? bs_descript { get; set; }
    }

    public class BibleContemplationDto
    {
        public BibleContemplationDto(int b_fk_id, string? bc_book, string? bc_chapter, string? bc_verse)
        {
            this.b_fk_id = b_fk_id;
            this.bc_book = bc_book;
            this.bc_chapter = bc_chapter;
            this.bc_verse = bc_verse;
        }
        public BibleContemplationDto(int bc_pk_id, int b_fk_id, DateTime? bc_date
            , string? bc_book, string? bc_chapter, string? bc_verse, string? bc_descript)
        {
            this.bc_pk_id = bc_pk_id;
            this.b_fk_id = b_fk_id;
            this.bc_date = bc_date;
            this.bc_book = bc_book;
            this.bc_chapter = bc_chapter;
            this.bc_verse = bc_verse;
            this.bc_descript = bc_descript;
        }

        public int bc_pk_id { get; set; }
        public int b_fk_id { get; set; }
        public DateTime? bc_date { get; set; }
        public string? bc_book { get; set; }
        public string? bc_chapter { get; set; }
        public string? bc_verse { get; set; }
        public string? bc_descript { get; set; }
    }


}
