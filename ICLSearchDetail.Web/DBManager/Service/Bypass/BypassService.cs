using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Service.Bypass
{
    public class BypassService
    {
        public String ReturnBypassUpdateAmountNoKey()
        {
            OutwardCheckService outSvcGetDate = new OutwardCheckService();

            string sqlUpdateCommand = @"UPDATE TBL_OUTWARD SET BATCH_ACCOUNT_NUMBER_FIN = '0000000000000', BATCH_ACCOUNT_NO_V_FLAG = '1'
                                    WHERE TRANSACTION_DATE = '" + outSvcGetDate.GetDateCurrentDate() + "' AND VALIDATION_STATUS = '3' AND CHK_FLAG = '3' AND BATCH_ACCOUNT_NO_V_FLAG IS NULL AND FLOW_TYPE = 'OCLGBS'";
            int numberOfRecords = 0;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateCommand, connection);
                connection.Open();
                numberOfRecords = cmd.ExecuteNonQuery();
            }
            return "  " + numberOfRecords + " row/s Updated";
        }

        public String ReturnBypassSelectTotalAmountNoKey()
        {
            string returnCount = "";
            OutwardCheckService outSvcGetDate = new OutwardCheckService();
            string sqlCommand = @"SELECT count(*) as CNT FROM TBL_OUTWARD WHERE TRANSACTION_DATE = '" + outSvcGetDate.GetDateCurrentDate() + "' AND VALIDATION_STATUS = '3' AND CHK_FLAG = '3' AND BATCH_ACCOUNT_NO_V_FLAG IS NULL AND FLOW_TYPE = 'OCLGBS'";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnCount = reader["CNT"].ToString();

                }
            }

            return returnCount;
        }
    }
}