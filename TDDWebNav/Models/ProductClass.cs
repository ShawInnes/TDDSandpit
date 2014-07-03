using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDDWebNav.Models
{
    public class ProductClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProductClass class.
        /// </summary>
        public ProductClass(int id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
            Products = new List<Product>();
        }
    }
}