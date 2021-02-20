using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5204_PassionProject_N01437602.Models
{
    public class ShowUser
    {
        public UserDto user { get; set; }

        public IEnumerable<ProductDto> userproducts { get; set; }

    }
}