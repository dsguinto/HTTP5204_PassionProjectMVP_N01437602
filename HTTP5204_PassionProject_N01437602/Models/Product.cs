using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTP5204_PassionProject_N01437602.Models
{
    public class Product
    {

        [Key]
        public int ProductID { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductCondition { get; set; }
        public string ProductGender { get; set; }
        public string ProductColour { get; set; }
        public string ProductType { get; set; }
        public string ProductSize { get; set; }
        public bool ProductHasPic { get; set; }

        //Gets extension of image if user has one
        public string ProductPicExtension { get; set; }

        //A product belongs to one user
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }

    public class ProductDto
    {
        public int ProductID { get; set; }
        [DisplayName("Price")]
        public decimal ProductPrice { get; set; }
        [DisplayName("Listing Name")]
        public string ProductName { get; set; }
        [DisplayName("Description")]
        public string ProductDesc { get; set; }
        [DisplayName("Condition")]
        public string ProductCondition { get; set; }
        [DisplayName("Gender")]
        public string ProductGender { get; set; }
        [DisplayName("Colour")]
        public string ProductColour { get; set; }
        [DisplayName("Type")]
        public string ProductType { get; set; }
        [DisplayName("Size")]
        public string ProductSize { get; set; }

        public bool ProductHasPic { get; set; }
        public string ProductPicExtension { get; set; }

        public int UserID { get; set; }
    }
}