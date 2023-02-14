using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMites_Engine._02.DTO._03.Fellowship
{
  public class Fellowship_DTO
  {
    public Fellowship_DTO() {
      FELLOWSHIP = new();
      FELLOWSHIP_DEPARTMENT = new();
      FELLOWSHIP_PHOTO = new();
      FELLOWSHIP_TESTIMONY = new();
    }
    ~Fellowship_DTO() { }
    public FELLOWSHIP FELLOWSHIP { get; set; }
    public FELLOWSHIP_DEPARTMENT FELLOWSHIP_DEPARTMENT { get; set; }
    public FELLOWSHIP_PHOTO FELLOWSHIP_PHOTO { get; set; }
    public FELLOWSHIP_TESTIMONY FELLOWSHIP_TESTIMONY { get; set; }
  }
}
