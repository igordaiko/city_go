using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace City_Go.Models
{
    public class HomeModel
    {
        public List<CategoriesModel> Categories { get; set; }
        public List<FilterModel> Filters { get; set; }
        public List<PlacesModel> Places { get; set; }
        public PagesInfo PagingInfo { get; set; }

    }
}