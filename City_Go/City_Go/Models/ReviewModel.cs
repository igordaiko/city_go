using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace City_Go.Models
{
    public class ReviewModel
    {
        public int ID { get; set; }
        public int Place_ID { get; set; }
        public int User_ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}