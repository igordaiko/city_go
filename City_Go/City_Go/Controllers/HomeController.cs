using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using City_Go.DataBase;
using City_Go.Models;
using System.Data;
using System.Windows.Forms;
namespace City_Go.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ITable itable = new Categories_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataTable table = reader.SelectAllRows();
            HomeModel model = new HomeModel();
            model.Categories = new List<CategoriesModel>();
            CategoriesModel cat_model;
            foreach (DataRow row in table.Rows)
            {
                cat_model = new CategoriesModel();
                cat_model.ID = (int)row[0];
                cat_model.Name = (string)row[1];
                cat_model.html_id = (string)row[2];
                model.Categories.Add(cat_model);                
            }
            
            return View(model);
        }
        public ActionResult Change_Filter(string id)
        {
            ITable itable = new Filter_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataTable table = reader.SelectRowsByContainsFilter(id, "cats");
            List<FilterModel> model = new List<FilterModel>();
            FilterModel filter_model;
            foreach(DataRow row in table.Rows)
            {
                filter_model = new FilterModel();
                filter_model.ID = (int)row[0];
                filter_model.Name = (string)row[1];
                filter_model.Description = (string)row[2];
                filter_model.Cat_id = (string)row[3];
                model.Add(filter_model);
            }
            ViewBag.Filters = model;
            return PartialView(model);
        }
        public ActionResult Give_Result(string category, string filters, double value_of_sum, int page = 1)
        {
            int page_size = 9;
            ITable itable = new Places_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataTable table_with_places = reader.SelectRowsByFilter(category, "categories");
            DataTable result_table = table_with_places.Clone();
            DataRow[] rows_array;
            string[] arr_of_filters = filters.Split(';');
            PlacesModel place;
            HomeModel model = new HomeModel();
            GenericListOfElements<PlacesModel> places_gen = new GenericListOfElements<PlacesModel>(itable);
            model.Places = places_gen.ListOfItems.Where(i => i.Categories.Contains(category)).ToList();
            filters = filters.Remove(filters.Length - 1, 1);
            string[] arr_filters = filters.Split(';');
            arr_filters.ToList().Sort(delegate (string x, string y) { return int.Parse(x).CompareTo(int.Parse(y)); });
            filters = string.Join(";", arr_filters);
            List<PlacesModel> result_places = model.Places.Where(i => i.Filters.Equals(filters)).ToList();
            if (result_places.Count < 50)
                model.Places = GiveListOfPlaces(model.Places, filters, value_of_sum).Take(50).Skip((page - 1) * page_size).Take(page_size).ToList();
            else
                model.Places = result_places;
            foreach (DataRow row in result_table.Rows)
            {
                place = new PlacesModel();
                place.ID = (int)row["id"];
                place.Name = (string)row["name"];
                place.Price = Convert.ToInt32((decimal)row["price"]);
                place.Img = (string)row["img"];
                place.Description = (string)row["description"];
                place.Address = (string)row["address"];
                model.Places.Add(place);
            }
            model.PagingInfo = new PagesInfo { CurrentPage = page, ItemsPerPage = page_size, TotalItems = 50 };

            return View(model);
        }

        private List<PlacesModel> GiveListOfPlaces(List<PlacesModel> result, string filters, double value_of_sum)
        {
            List<PlacesModel> result_list = result;
            string[] arr_of_filters = filters.Split(';');
            arr_of_filters.ToList().Sort(delegate (string x, string y) { return int.Parse(x).CompareTo(int.Parse(y)); });
            bool b = true;
            foreach (string filter in arr_of_filters)
                if(filter != "")
                    result_list = result_list.Where(i => i.Filters.Contains(filter)).ToList();

            if (result_list.Count < 50)
            {
                arr_of_filters = arr_of_filters.Take(arr_of_filters.Length - 1).ToArray();
                filters = String.Join(";", arr_of_filters);
                result_list.Sort(delegate (PlacesModel x, PlacesModel y) { return x.Price.CompareTo(y.Price); });
                result_list.AddRange(GiveListOfPlaces(result, filters, value_of_sum));
            }

            return result_list.Distinct().ToList();
        }
        public ActionResult Info_Page(int id)
        {
            ITable itable = new Places_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataRow row_with_place = reader.SelectRowsByFilter(id.ToString(), "id").Rows[0];
            PlacesModel model = new PlacesModel();

            model.ID = (int)row_with_place["id"];
            model.Name = (string)row_with_place["name"];
            //model.Images = row_with_place["images"].ToString().Split(';').ToString();
            model.FullDescription = (string)row_with_place["full_description"];
            model.Contacts = (string)row_with_place["contacts"];

            return View(model);
        }

        public ActionResult Reviews(string place_id)
        {
            ITable itable = new Reviews_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataTable table = reader.SelectRowsByFilter(place_id, "place_id");
            ReviewModel review;
            PlacesModel model = new PlacesModel();
            model.Reviews = new List<ReviewModel>();
            foreach (DataRow row in table.Rows)
            {
                review = new ReviewModel();
                review.ID = (int)row["id"];
                review.Place_ID = (int)row["place_id"];
                review.User_ID = (int)row["user_id"];
                review.Text = (string)row["text"];
                review.Date = (DateTime)row["date"];
                model.Reviews.Add(review);
            }
            
            return PartialView(model);
        }
    }
}