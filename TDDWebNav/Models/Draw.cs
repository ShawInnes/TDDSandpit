using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDDWebNav.Models
{
    public class Draw
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DrawNo { get; set; }
        public string FormattedDrawNo { get { return string.Format("Draw #{0:0000}", DrawNo); } }
    }
}
