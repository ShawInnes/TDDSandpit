using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDDWebNav.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int ProductClassId {get; set;}
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProductClass class.
        /// </summary>
        public Product(int productClassId, int id, string code, string name)
        {
            ProductClassId = productClassId;
            Id = id;
            Code = code;
            Name = name;
        }
    }
}