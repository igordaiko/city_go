using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace City_Go.Models
{
    public class PlacesModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Categories { get; set; }
        public string Filters { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Address { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string FullDescription { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Contacts { get; set; }
        public string Images { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Work_Time { get; set; }
        public int Visitors { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public double Rating { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Metrostations { get; set; }
        public List<ReviewModel> Reviews { get; set; }
        public string New_or_no { get; set; }

    }
}