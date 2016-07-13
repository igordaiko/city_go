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
    public class WorkWithDataBase
    {
        SqlDataAdapter adapter;
        DataSet set;
        string con;
        ITable table;
        public WorkWithDataBase(ITable table)
        {
            con = DataBaseInfo.GetDataBaseConnectionString();
            this.adapter = table.Adapter;
            this.table = table;
            set = new DataSet();
            adapter.Fill(set);
        }

        public DataTable SelectAllRows()
        {
            DataTable result = set.Tables[0].Clone();
            foreach (DataRow row in set.Tables[0].Rows)
            {
                result.ImportRow(row);
            }
            return result;
        }
        public DataTable SelectRowsByFilter(string filter, string column)
        {
            DataTable result = set.Tables[0].Clone();
            foreach(DataRow row in set.Tables[0].Rows)
            {
                if (row[column].ToString().Equals(filter))
                    result.ImportRow(row);
            }
            return result;
        }
        public DataTable SelectRowsByContainsFilter(string filter, string column)
        {
            DataTable result = set.Tables[0].Clone();
            foreach (DataRow row in set.Tables[0].Rows)
            {
                if (row[column].ToString().Contains(filter))
                    result.ImportRow(row);
            }
            return result;
        }

        public void AddRow(object[] colls)
        {
            DataTable table = SelectAllRows();
            DataRow row = table.NewRow();
            int i = 0;
            foreach (object coll in colls)
            {
                row[i] = coll;
                i++;
            }
            table.Rows.Add(row);
            using (new SqlCommandBuilder(adapter))
            {
                adapter.Update(table);
            }
        }
        public void AddRow(object value, string coll_name, int id)
        {
            DataTable table = SelectAllRows();
            DataRow row = table.NewRow();
            int i = 0;
            row[0] = id;
            row[coll_name] = value;
            table.Rows.Add(row);
            using (new SqlCommandBuilder(adapter))
            {
                adapter.Update(table);
            }
        }
        public void UpdateRow(string col_name, object value, int id, string table_name)
        {
            SqlConnection connection = new SqlConnection(con);
            string query = "Update " + table_name + " set " + col_name + " = @p1 where id = " + id.ToString();
            SqlCommand command = new SqlCommand(query, connection);
            SqlParameter parameter;
            DataTable result_table = SelectAllRows().Clone();
            parameter = new SqlParameter("@p1", value);
            command.Parameters.Add(parameter);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateRow(object[] colls)
        {
            SqlConnection connection = new SqlConnection(con);
            string query = table.queryUpdate;
            SqlCommand command = new SqlCommand(query, connection);
            SqlParameter parameter;
            for (int i = 0; i < colls.Length; i++)
            {
                parameter = new SqlParameter("@p" + (i + 1), colls[i]);
                command.Parameters.Add(parameter);
            }
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteRow(int id, string table_name)
        {
            SqlConnection connection = new SqlConnection(con);
            string query = string.Format("delete {0} where id = {1}", table_name, id);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public int CreateId()
        {
            int id, i = 100;
            do
            {
                id = Math.Abs(new Random().Next(0, i) * Int32.Parse(DateTime.Now.Millisecond.ToString()));
                i *= 10;
            }
            while (set.Tables[0].Rows.Contains(id));
            
            return id;
        }
    }
}