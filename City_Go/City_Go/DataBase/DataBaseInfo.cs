using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City_Go.DataBase
{

        static public class DataBaseInfo
        {
            private static string DataBaseConnectionString
            {
                get
                {
                    return "Data Source=SQL5018.Smarterasp.net;Initial Catalog=DB_9FD6DF_citygodatabase;User Id=DB_9FD6DF_citygodatabase_admin;Password=genius1997;";
                }
            }
            private static string CommentsXmlFileConnectionString
            {
                get
                {
                    return "/App_Data/Comments.xml";
                }
            }
            static public string GetDataBaseConnectionString()
            {
                return DataBaseConnectionString;
            }
            static public string GetCommentsConnectionString()
            {
                return CommentsXmlFileConnectionString;
            }
        }
}