using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using City_Go.DataBase;
using System.ComponentModel.DataAnnotations;
using City_Go.Models;
using WebMatrix.WebData;
namespace City_Go.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin

        public ActionResult Index(int page = 1)
        {

            int page_size = 15;
            ITable itable = new Places_Reader();
            GenericListOfElements<PlacesModel> gen_model = new GenericListOfElements<PlacesModel>(itable);
            HomeModel model = new HomeModel();
            model.Places = gen_model.ListOfItems;
            var prods = model.Places.Skip((page - 1) * page_size).Take(page_size);
            model.Places = new List<PlacesModel>();
            foreach (var p in prods)
                model.Places.Add(p);
            model.PagingInfo = new PagesInfo { CurrentPage = page, ItemsPerPage = page_size, TotalItems = gen_model.ListOfItems.Count };

            return View(model);
        }


        //---Работа с местами---
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPlace(PlacesModel place, List<string> categories, List<string> filters, HttpPostedFileBase img_file, int page = 1)
        {
            ITable itable = new Places_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            PlacesModel result = place;
            result.Categories = GiveIdsByNameList(categories, new Categories_Reader());
            result.Filters = GiveIdsByNameList(filters, new Filter_Reader());
            if(img_file != null)
            { 
                img_file.SaveAs(Server.MapPath("/img/" + img_file.FileName));
                result.Img = "/img/" + img_file.FileName;
            }
            GenericListOfElements<PlacesModel> gen_model = new GenericListOfElements<PlacesModel>(itable, result);
            if (place.New_or_no.Equals("new"))
                reader.AddRow(gen_model.ParametersToUpdate);
            else
                reader.UpdateRow(gen_model.ParametersToUpdate);

            return EditPlace(place.ID);
        }
        
        public ActionResult EditPlace(int? id)
        {
            ITable itable = new Places_Reader();
            PlacesModel model;
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            if (id != null)
            {
                DataRow row = reader.SelectRowsByFilter(id.ToString(), "id").Rows[0];
                GenericListOfElements<PlacesModel> gen_model = new GenericListOfElements<PlacesModel>(itable, id.ToString());

                model = gen_model.Model;
                itable = new Reviews_Reader();

                GenericListOfElements<ReviewModel> review_gen_model = new GenericListOfElements<ReviewModel>(itable, model.ID.ToString(), "place_id");
                model.Reviews = review_gen_model.ListOfItems;
                model.Categories = GiveNameByIdsList(model.Categories, new Categories_Reader());
                model.Filters = GiveNameByIdsList(model.Filters, new Filter_Reader());

                model.New_or_no = "no";

                itable = new Users_Reader();
                GenericListOfElements<UserModel> users_gen;
                foreach (ReviewModel review in model.Reviews)
                {
                    users_gen = new GenericListOfElements<UserModel>(itable, review.ID.ToString());
                    review.Name = users_gen.Model.Name;
                }
            }
            else
            {
                model = new PlacesModel();
                model.Reviews = new List<ReviewModel>();
                model.Categories = "";
                model.Filters = "";
                model.Images = "";
                model.New_or_no = "new";
                model.ID = reader.CreateId();
            }
            itable = new Categories_Reader();
            GenericListOfElements<CategoriesModel> cats_gen = new GenericListOfElements<CategoriesModel>(itable);
            ViewBag.Cats_List = cats_gen.ListOfItems;

            itable = new Filter_Reader();
            GenericListOfElements<FilterModel> filter_gen = new GenericListOfElements<FilterModel>(itable);
            ViewBag.Filters_List = filter_gen.ListOfItems;


            return View(model);
        }
        //End---Работа с местами---


        //---Работа с категориями и фильтрами--
        public ActionResult CategoriesAndFilters()
        {
            CategoriesAndFiltersPageModel model = new CategoriesAndFiltersPageModel();
            ITable itable = new Categories_Reader();
            GenericListOfElements<CategoriesModel> gen_cats = new GenericListOfElements<CategoriesModel>(itable);
            model.Categories = gen_cats.ListOfItems;

            itable = new Filter_Reader();
            GenericListOfElements<FilterModel> gen_filters = new GenericListOfElements<FilterModel>(itable);
            model.Filters = gen_filters.ListOfItems;

            return View(model);
        }

        [HttpPost]
        public ActionResult CategoriesAndFilters(string description, string name, string cats, string table, int? id, string new_or_no)
        {
            ITable itable;
            if (table.Equals("filter"))
            {
                itable = new Filter_Reader();
                FilterModel model = new FilterModel();
                WorkWithDataBase reader = new WorkWithDataBase(itable);
                bool b = true;
                if (reader.SelectRowsByFilter(id.ToString(), "id").Rows.Count == 0) 
                    b = false;

                model.ID = (int)id;
                model.Name = name;
                model.Description = description;
                model.Cat_id = cats;
                GenericListOfElements<FilterModel> gen_filters = new GenericListOfElements<FilterModel>(itable, model);
                if (b)
                    reader.UpdateRow(gen_filters.ParametersToUpdate);
                else
                    reader.AddRow(gen_filters.ParametersToUpdate);
            }
            else
            {
                itable = new Categories_Reader();
                CategoriesModel model = new CategoriesModel();
                WorkWithDataBase reader = new WorkWithDataBase(itable);
                bool b = true;
                if (reader.SelectRowsByFilter(id.ToString(), "id").Rows.Count == 0) 
                    b = false;
                      
                model.ID = (int)id;
                model.Name = name;
                //cats потому что не хочу добавлять еще один параметр. Просто в cats буду записывать html_id
                model.html_id = cats;
                GenericListOfElements<CategoriesModel> gen_cats = new GenericListOfElements<CategoriesModel>(itable, model);
                if (b)
                    reader.UpdateRow(gen_cats.ParametersToUpdate);
                else
                    reader.AddRow(gen_cats.ParametersToUpdate);
            }
            return CategoriesAndFilters();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public void DeleteCategoriesFilters(string table, int id)
        {
            ITable itable;
            WorkWithDataBase reader;
            if(table == "category")
                itable = new Categories_Reader();
            else
                itable = new Filter_Reader();

            reader = new WorkWithDataBase(itable);
            reader.DeleteRow(id, table);
        }
        //End---Работа с категориями и фильтрами--



        //---Вспомогательные методы.---
        [HttpPost]
        [Authorize(Roles = "admin")]
        public void DeletePlace(int id)
        {
            ITable itable = new Places_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            reader.DeleteRow(id, "Places");
        }
        [HttpPost]
        public void SaveNewImage(int? place_id)
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("/img/" + fileName));
                    
                    ITable itable = new Places_Reader();
                    WorkWithDataBase reader = new WorkWithDataBase(itable);

                    if (place_id != null)
                    {
                        string value = reader.SelectRowsByFilter(place_id.ToString(), "id").Rows[0]["Images"].ToString() + "/img/" + fileName.ToString() + ";";
                        reader.UpdateRow("images", value, (int)place_id, "Places");
                    }
                        
                    else
                    {
                        place_id = reader.CreateId();
                        reader.AddRow("/img/" + fileName.ToString() + ";", "images", (int)place_id);
                    }
                }

            }
        }
        [Authorize(Roles = "admin")]
        public void DeleteImg(string link, int place_id)
        {
            ITable itable = new Places_Reader();
            WorkWithDataBase reader = new WorkWithDataBase(itable);
            DataRow row = reader.SelectRowsByFilter(place_id.ToString(), "id").Rows[0];
            string images = row["images"].ToString();
            images = images.Remove(images.IndexOf(link), link.Length + 1);
            reader.UpdateRow("images", images, place_id, "Places");
            System.IO.File.Delete(Server.MapPath(link));
        }

        private string GiveNameByIdsList(string ids, ITable itable)
        {
            string result = "";

            if (itable is Categories_Reader)
            {
                GenericListOfElements<CategoriesModel> _gen = new GenericListOfElements<CategoriesModel>(itable);
                List<CategoriesModel> list = _gen.ListOfItems;
                foreach (string str in ids.Remove(ids.Length - 1, 1).Split(';'))
                    result += list[int.Parse(str)].Name + ";";
            }
            else if (itable is Filter_Reader)
            {
                GenericListOfElements<FilterModel> _gen = new GenericListOfElements<FilterModel>(itable);
                List<FilterModel> list = _gen.ListOfItems;
                foreach (string str in ids.Remove(ids.Length - 1, 1).Split(';'))
                    result += list[int.Parse(str)].Description + ";";
            }
            else if (itable is Images_Reader)
            {
                GenericListOfElements<ImageModel> _gen = new GenericListOfElements<ImageModel>(itable);
                List<ImageModel> list = _gen.ListOfItems;
                if(ids.Length > 1)
                    foreach (string str in ids.Remove(ids.Length - 1, 1).Split(';'))
                    result += list.Where(i => i.ID == int.Parse(str)).Select(i => i.Url).ToList()[0] + ";";

            }
            return result;
        }
        private string GiveIdsByNameList(List<string> names_list, ITable itable)
        {
            string result = "";

            if (itable is Categories_Reader)
            {
                GenericListOfElements<CategoriesModel> _gen = new GenericListOfElements<CategoriesModel>(itable);
                List<CategoriesModel> list = _gen.ListOfItems;
                foreach(string str in names_list)
                {
                    result += list.Where(i => i.Name == str).Select(id => id.ID).ToList()[0].ToString() + ";";
                }
            }
            else if (itable is Filter_Reader)
            {
                GenericListOfElements<FilterModel> _gen = new GenericListOfElements<FilterModel>(itable);
                List<FilterModel> list = _gen.ListOfItems;
                foreach (string str in names_list)
                {
                    result += list.Where(i => i.Description == str).Select(id => id.ID).ToList()[0].ToString() + ";";
                }
            }

            return result;
        }
       
        //End.---Вспомогательные методы.---



    }
}