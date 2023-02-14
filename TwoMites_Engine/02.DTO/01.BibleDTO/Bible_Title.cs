using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._02.DTO._01.BibleDTO
{
  public class Bible_Title
  {
    public Bible_Title()
      : this("","","")
    { }

    public Bible_Title(string bt_id, string bt_name, string bt_name_key)
    {
      this.bt_id = bt_id;
      this.bt_name = bt_name;
      this.bt_name_key = bt_name_key;
    }

    public string? bt_id { get; set; }
    public string? bt_name { get; set; }
    public string? bt_name_key { get; set; }


  }
}
