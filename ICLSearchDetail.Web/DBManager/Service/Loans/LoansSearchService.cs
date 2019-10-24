using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Service.Loans
{
    public class LoansSearchService
    {
        public String GetDateCurrentDate()
        {
            var sqlString = "SELECT CONVERT(char(10), CUR_DATE,126) as CUR_DATE from tbl_eod_bod_stat;";
            string curr_date = "";
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string DateString = reader["CUR_DATE"].ToString();

                        curr_date = DateString.ToString();
                       
                    }
                }
            }
            catch (Exception e)
            {
                curr_date = "error occured";
            }
            return curr_date;
        }
    }
}