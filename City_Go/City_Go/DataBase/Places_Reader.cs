using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace City_Go.DataBase
{
    public class Places_Reader : ITable
    {
        public SqlDataAdapter adapter;
        SqlCommand command;
        SqlConnection connection;
        DataSet set;
        string query, con;
        public Places_Reader()
        {
            query = "Select * from Places";
            command = new SqlCommand(query);
            con = DataBaseInfo.GetDataBaseConnectionString();
            connection = new SqlConnection(con);
            adapter = new SqlDataAdapter(query, connection);
            Adapter = adapter;
            queryUpdate = "Update Places SET [name] = @p2,"+
                      " [categories] = @p3, " +
                      "[filters] = @p4, " +
                      "[description] = @p5, " +
                      "[img] = @p6, " +
                      "[price] = @p7, " +
                      "[address] = @p8, " +
                      "[full_description] = @p9, " +
                      "[contacts] = @p10, " +
                      "[images] = @p11, " +
                      "[work_time] = @p12, " +
                      "[visitors] = @p13, " +
                      "[likes] = @p14, " +
                      "[dislikes] = @p15 " +
                "where [id] = @p1";
        }
        public SqlDataAdapter Adapter { get; set; }
        public string queryUpdate { get; set; }

    }
}