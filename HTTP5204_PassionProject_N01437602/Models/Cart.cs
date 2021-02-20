using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTP5204_PassionProject_N01437602.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int CartQuantity { get; set; }
        public int CartCost { get; set; }

        //A product belongs to one user
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }


        //A Cart can have Many Products
        public ICollection<Product> Product { get; set; } 
    }

    public class CartDto
    {
        public int CartID { get; set; }
        public int CartQuantity { get; set; }
        public int CartCost { get; set; }

        public int UserID { get; set; }
    }
}