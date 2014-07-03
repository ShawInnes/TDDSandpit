using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDDWebNav.Models
{
    public class Products
    {
        private List<Product> list = new List<Product>();

        public Products()
        {
            list.Add(new Product(1, 1, "SATL", "TattsLotto"));
            list.Add(new Product(1, 2, "OZLO", "Oz Lotto"));
            list.Add(new Product(2, 3, "PBAL", "Powerball"));
            list.Add(new Product(3, 4, "SU66", "Super 66"));
            list.Add(new Product(1, 5, "POOL", "The Pools"));
            list.Add(new Product(1, 6, "MWLO", "Monday & Wednesday Lotto"));
            list.Add(new Product(4, 7, "$2Jk", "Super Jackpot"));
            list.Add(new Product(4, 8, "$5Jk", "Mega Jackpot"));
            list.Add(new Product(5, 9, "LS", "Lotto Strike"));
            list.Add(new Product(1, 10, "WEDL", "Wednesday Lotto"));
            list.Add(new Product(6, 11, "KENO", "SaKeno"));
            list.Add(new Product(7, 12, "KNCT", "CoinToss"));
            list.Add(new Product(1, 15, "MW&S", "Multi Product"));
            list.Add(new Product(9, 16, "INST", "Instant Scratch-Its"));
            list.Add(new Product(4, 17, "$2CT", "$2 Casket"));
            list.Add(new Product(0, 18, "BODR", "Bonus Draws"));
        }

        public List<Product> GetData()
        {
            return list;
        }
    }
}
