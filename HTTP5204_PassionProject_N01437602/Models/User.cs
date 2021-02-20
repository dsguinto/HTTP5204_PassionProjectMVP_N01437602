using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTP5204_PassionProject_N01437602.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserName { get; set; }
        public string UserLocation { get; set; }
        public string UserBio { get; set; }
        public bool UserHasPic { get; set; }


        //Gets extension of image if user has one
        public string UserPicExtension { get; set; }

        //A User can own many products
        public ICollection<Product> Product { get; set; }

    }


        public class UserDto
        {
            public int UserID { get; set; }
            
            [DisplayName("First Name")]
            public string UserFirstName { get; set; }
            [DisplayName("Last Name")]
            public string UserLastName { get; set; }
            [DisplayName("Username")]
            public string UserName { get; set; }
            [DisplayName("Location")]
            public string UserLocation { get; set; }
            [DisplayName("Bio")]
            public string UserBio { get; set; }

            public bool UserHasPic { get; set; }
            public string UserPicExtension { get; set; }

        }
}