using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace City_Go.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
    }
}