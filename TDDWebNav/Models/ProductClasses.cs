using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDDWebNav.Models
{
    public class ProductClasses
    {
        private List<ProductClass> list = new List<ProductClass>();
        public ProductClasses()
        {
            list.Add(new ProductClass(4, "Dl", "Draw Lotto Game"));
            list.Add(new ProductClass(7, "Ht", "Keno Coin Toss Game"));
            list.Add(new ProductClass(6, "Kn", "Keno Standard Game"));
            list.Add(new ProductClass(5, "Ls", "Lotto Strike Game"));
            list.Add(new ProductClass(1, "Lt", "Lotto Game"));
            list.Add(new ProductClass(2, "Pb", "Powerball Game"));
            list.Add(new ProductClass(3, "Sx", "Super 66 Game"));
        }

        public List<ProductClass> GetData()
        {
            return list;
        }
    }
}
