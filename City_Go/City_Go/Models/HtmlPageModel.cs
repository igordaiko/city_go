using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace City_Go.Models
{
    public class HtmlPageModel
    {
        public FileInfo ViewFile { get; set; }
        [AllowHtml]
        public string HtmlCode { get; set; }
    }
}