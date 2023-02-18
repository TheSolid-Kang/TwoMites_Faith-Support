using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._02.DTO._01.BibleDTO
{
  public class BibleDTO
  {
  }
  public enum EDTO_BIBLE : int {
  b_id ,b_book ,b_chapter
  ,b_verse  ,b_descript ,b_full_descript
}
  public class DTO_BIBLE
  {
    public DTO_BIBLE()
      : this("", "", "", "", "", "")
    { }
    public DTO_BIBLE(string b_book, string b_chapter, string b_verse)
      : this(b_book, b_chapter, b_verse, "", "")
    {
      this.b_book = b_book;
      this.b_chapter = b_chapter;
      this.b_verse = b_verse;
    }    
    public DTO_BIBLE(string b_book, string b_chapter, string b_verse, string b_descript, string b_full_descript)
    {
      this.b_book = b_book;
      this.b_chapter = b_chapter;
      this.b_verse = b_verse;
      this.b_descript = b_descript;
      this.b_full_descript = b_full_descript;
    }
    public DTO_BIBLE(string b_id, string b_book, string b_chapter, string b_verse, string b_descript, string b_full_descript)
      :this(b_book, b_chapter, b_verse, b_descript, b_full_descript)
    {
      this.b_id = b_id;
    }
    public string? b_id { get; set; }
    public string? b_book { get; set; }
    public string? b_chapter { get; set; }
    public string? b_verse { get; set; }
    public string? b_descript { get; set; }
    public string? b_full_descript { get; set; }
  }

  public class DTO_BIBLE_SUMMARY
  {
    public DTO_BIBLE_SUMMARY()
      : this(DateTime.Now, "", "", "", "")
    { }
    public DTO_BIBLE_SUMMARY(DateTime bs_date, string bs_descript)
      : this(bs_date,"","","",bs_descript)
    { }
    public DTO_BIBLE_SUMMARY(DateTime bs_date
    , string bs_book
    , string bs_chapter
    , string bs_verse
    , string bs_descript
      )
    {
      this.bs_date = bs_date;
      this.bs_book = bs_book;
      this.bs_chapter = bs_chapter;
      this.bs_verse = bs_verse;
      this.bs_descript = bs_descript;
    }
    public DateTime? bs_date { get; set; }
    public string? bs_book { get; set; }
    public string? bs_chapter { get; set; }
    public string? bs_verse { get; set; }
    public string? bs_descript { get; set; }
  }

  public class DTO_BIBLE_CONTEMPLATION
  {
    public DTO_BIBLE_CONTEMPLATION()
      : this(DateTime.Now, "", "", "", "")
    { }
    public DTO_BIBLE_CONTEMPLATION(DateTime bc_date, string bc_descript)
      : this(bc_date, "", "", "", bc_descript)
    { }
    public DTO_BIBLE_CONTEMPLATION(DateTime bc_date
    , string bc_book
    , string bc_chapter
    , string bc_verse
    , string bc_descript
      )
    {
      this.bc_date = bc_date;
      this.bc_book = bc_book;
      this.bc_chapter = bc_chapter;
      this.bc_verse = bc_verse;
      this.bc_descript = bc_descript;
    }

    public DateTime? bc_date { get; set; }
    public string? bc_book { get; set; }
    public string? bc_chapter { get; set; }
    public string? bc_verse { get; set; }
    public string? bc_descript { get; set; }
  }


}
