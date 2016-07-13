using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using City_Go.DataBase;
//using City_Go.Models.AdminModels;
namespace City_Go.Models
{
    /// <summary>
    /// Класс предназначен для получение списка с типом Модели Т. 
    /// </summary>
    /// <typeparam name="T">Имя класса модели</typeparam>
    public class GenericListOfElements<T>
    {
        ITable itable;
        WorkWithDataBase reader;
        DataTable table;
        T model;
        public GenericListOfElements(ITable itable)
        {
            this.itable = itable;
            reader = new WorkWithDataBase(this.itable);
            table = reader.SelectAllRows();
            ListOfItems = ReturnList();
        }
        //Конструктор, который заполняет масив параметров для обновления
        public GenericListOfElements(ITable itable, T model)
        {
            this.itable = itable;
            reader = new WorkWithDataBase(this.itable);
            table = reader.SelectAllRows();
            this.model = model;
            ParametersToUpdate = GetParametersToUpdate();
        }
        //Конструктор предназначен для выбора элементов соответсвующих критериям поиска
        public GenericListOfElements(ITable itable, string find, string row_name)
        {
            this.itable = itable;
            reader = new WorkWithDataBase(this.itable);
            table = reader.SelectRowsByFilter(find, row_name);
            ListOfItems = ReturnList();

        }
        //Конструктор предназначен для выбора по определенным колонкам
        //public GenericListOfElements(ITable itable, string[] columns_)
        //{
        //    this.itable = itable;
        //    reader = new WorkWithDataBase(this.itable);
        //    table = reader.SelectAllRows(columns_);
        //    ListOfItems = ReturnList();
        //}
        //Конструктор предназначен для заполнения модели одного элемента
        public GenericListOfElements(ITable itable, string id)
        {
            this.itable = itable;
            reader = new WorkWithDataBase(this.itable);
            table = reader.SelectRowsByFilter(id, "id");
            Model = ReturnModel();
        }

        //Коллекция типа Модели
        public List<T> ListOfItems { get; set; }
        public T Model { get; set; }
        public object[] ParametersToUpdate { get; set; }
        private List<T> ReturnList()
        {
            List<T> list = new List<T>();
            T model;
            Type type;
            foreach (DataRow row in table.Rows)
            {
                //Создание экземляра класса модели
                model = (T)Activator.CreateInstance(typeof(T));
                //Нужно для рефлексии
                type = typeof(T);
                //Получение набора свойств
                PropertyInfo[] props = type.GetProperties();
                for(int i = 0; i < table.Columns.Count; i++)
                {
                    //Устанавливаю значение свойства за индексом
                   props[i].SetValue(model, row[i]);
                }
                list.Add(model);
            }
            return list;   
        }
        private T ReturnModel()
        {
            T model;
            Type type;
            model = (T)Activator.CreateInstance(typeof(T));
            type = typeof(T);
            DataRow row = table.Rows[0];
            //Получение набора свойств
            PropertyInfo[] props = type.GetProperties();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                //Устанавливаю значение свойства за индексом
                props[i].SetValue(model, row[i]);
            }
            return model;
        }
        private object[] GetParametersToUpdate()
        {
            Type type = model.GetType();
            PropertyInfo[] props = type.GetProperties();
            object[] result = new object[table.Columns.Count];
            
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (props[i].GetValue(model) != null)
                    result[i] = props[i].GetValue(model);
                else if (props[i].GetType() == typeof(System.Int32) || props[i].GetType() == typeof(System.Decimal))
                    result[i] = 0;
                else
                    result[i] = "";
            }
            return result;
        }
    }
}